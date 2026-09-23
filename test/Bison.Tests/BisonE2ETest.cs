using System.Net.Http.Json;
using System.Threading.Tasks;
using SimpleDB;
using System.Text;
using Bison.Taxonomy;


public class BisonE2ETests()
{
    /**
        * Test that the "read" command correctly outputs the expected author name "mivh" to the console.
    */
    [Fact]
    public void CommandReadTest()
    {
        // Arrange
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            // Act
            Program.Main(new[] { "read" });

            // Assert
            Assert.Contains("mivh", writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /**
        * Test that the "comment" command correctly outputs the expected author name "mivh" to the console.
    */
    [Fact]
    public async Task CommandObservationTest()
    {
        // Setup
        var baseURL = "http://localhost:5189";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);

        // Arrange
        // var obsDb = CsvDatabase<UniqueObservation>.GetInstance("bison_observe_cli_db.csv");
        // var countBefore = obsDb.Read().ToList().Count;
        var responseBefore = await client.GetFromJsonAsync<IEnumerable<UniqueObservation>>("/observations");

        int countBefore = responseBefore.ToList().Count();


        // Act
        Program.Main(new[] { "observe", "test message", "test location" });

        // Assert
        var responseAfter = await client.GetFromJsonAsync<IEnumerable<UniqueObservation>>("/observations");
        var countAfter = responseAfter.ToList().Count();
        Assert.Equal(countBefore + 1, countAfter);

        var stored = responseAfter.Last();
        Assert.Equal("test message", stored.Message);
        Assert.Equal("test location", stored.Location);
    }

    /// <summary>
    /// Tests that the "location" command returns observations for a known location.
    /// </summary>
    [Fact]
    public void CommandLocationReturnsMatchesTest()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            Program.Main(new[] { "location", "eee" });
            Assert.Contains("mivh", writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /// <summary>
    /// Tests that the "location" command returns a message indicating that there are no observations for an unknown location.
    /// </summary>
    [Fact]
    public void CommandLocationReturnsNoneForUnknownLocationTest()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            Program.Main(new[] { "location", "nowhere-that-exists" });
            Assert.Contains("No observations on this location", writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    // helper methods 
    static readonly char[] FuzzString = { ',', '"', '\'', '\n', '\r', '\t', '\\', '=', ';', '\0'};
    static readonly string[] FuzzProposal = {
        "Årefodede",
        "Hejrer",
        "Fregatfugle",
        "Pelikaner",
        "Skarver",
        "Suler",
        "Ibiser & skestorke",
        "Sølvhejre",
        "Fiskehejre",
        "Kohejre",
        "Purpurhejre",
        "Tophejre"
    };

    static string RandomString(Random rng, int maxLen = 50)
    {
        var len = rng.Next(0, maxLen);
        var sb = new StringBuilder(len);
        for (int i = 0; i < len; i++)
        {
            // 30% chance of a character that's likely to break something
            sb.Append(rng.Next(100) < 30
                ? FuzzString[rng.Next(FuzzString.Length)]
                : (char)rng.Next(32, 127));
        }
        return sb.ToString();
    }


    /// <summary>
    /// E2E Fuzz testing on observation command using random string input
    /// </summary>
    [Fact]
    [Trait("Category", "Fuzz")]
    public async Task FuzzE2EObservations() {

        // Setup
        var baseURL = "http://localhost:5189";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);

        var seed = 6767;
        var rng = new Random(seed);
        var expected = new List<UniqueObservation>();

        for (int i = 0; i < 5; i++)
        {
            var obs = new GenericObservation(
                Author: RandomString(rng), 
                Message: RandomString(rng),
                Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Location: RandomString(rng)
            );

            var response = await client.PostAsJsonAsync("/observation", obs);

            var created = await response.Content.ReadFromJsonAsync<UniqueObservation>();
            expected.Add(created!);
        }   

        var all = await client.GetFromJsonAsync<UniqueObservation[]>("/observations");

        foreach (var exp in expected)
        {
            // find matching id between actual and expected
            var act = all!.SingleOrDefault(o => o.Id == exp.Id);
            //see if aut, msg and loc match
            Assert.True(act is not null, $"observation {exp.Id} missing after round-trip");
            Assert.Equal(exp.Message, act!.Message);
            Assert.Equal(exp.Author, act.Author);
            Assert.Equal(exp.Location, act.Location);
        }
    }

    /// <summary>
    /// E2E Fuzz testing on comment endpoint using random string input.
    /// </summary>
    [Fact]
    [Trait("Category", "Fuzz")]
    public async Task FuzzE2EComment()
    {
        // Setup
        var baseURL = "http://localhost:5189";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);

        var seed = 6767;
        var rng = new Random(seed);
        var expected = new List<Comment>();

        for (int i = 0; i < 5; i++)
        {
            var cmt = new Comment(
                Id: rng.Next(1, 10),
                Author: RandomString(rng),
                Message: RandomString(rng),
                Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            );

            var response = await client.PostAsJsonAsync("/comment", cmt);
            Assert.True(response.IsSuccessStatusCode,
                $"POST failed at i={i} (seed {seed}) for observation {cmt.Id}: {response.StatusCode}");

            expected.Add(cmt);
        }

        // Verify: group by observation id so we only GET once per observation
        foreach (var group in expected.GroupBy(c => c.Id))
        {
            var all = await client.GetFromJsonAsync<Comment[]>($"/comments?id={group.Key}");

            foreach (var exp in group)
            {
                var match = all!.Any(a => a.Message == exp.Message && a.Author == exp.Author);
                Assert.True(match,
                    $"comment on observation {exp.Id} missing after round-trip (seed {seed})");
            }
        }
    }

    /// <summary>
    /// E2E Fuzz testing on proposal endpoint using random taxa from the taxonomy.
    /// </summary>
    [Fact]
    [Trait("Category", "Fuzz")]
    public async Task FuzzE2EProposal()
    {
        // Setup
        var baseURL = "http://localhost:5189";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);

        TaxonTree taxonomy = TaxonTree.LoadFromEmbeddedResource();

        var seed = 6767;
        var rng = new Random(seed);
        var expected = new List<Proposal>();

        for (int i = 0; i < 5; i++)
        {
            var name = FuzzProposal[rng.Next(FuzzProposal.Length)];
            var tax = taxonomy.GetByVernacularName(name);
            Assert.True(tax is not null, $"taxon '{name}' not found in taxonomy");

            var proposal = new Proposal(
                ObservationId: rng.Next(1, 10),
                Author: RandomString(rng),
                TaxonId: tax!.TaxonId,
                Timestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            );

            var response = await client.PostAsJsonAsync("/proposal", proposal);
            Assert.True(response.IsSuccessStatusCode,
                $"POST failed at i={i} (seed {seed}) for observation {proposal.ObservationId}: {response.StatusCode}");

            expected.Add(proposal);
        }

        // Verify: group by observation id so we only GET once per observation
        foreach (var group in expected.GroupBy(p => p.ObservationId))
        {
            var all = await client.GetFromJsonAsync<Proposal[]>($"/proposals?id={group.Key}");

            foreach (var exp in group)
            {
                var match = all!.Any(a => a.TaxonId == exp.TaxonId && a.Author == exp.Author);
                Assert.True(match,
                    $"proposal on observation {exp.ObservationId} missing after round-trip (seed {seed})");
            }
        }
    }
}