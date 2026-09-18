using System.Net.Http.Json;
using System.Threading.Tasks;
using SimpleDB;


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
}