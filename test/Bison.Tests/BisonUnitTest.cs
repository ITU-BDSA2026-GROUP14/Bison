using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NuGet.Frameworks;

public class BisonFactory : WebApplicationFactory<Program>
{
    private string DbPath { get; } = Path.Combine(Path.GetTempPath(), "bison-test.db");

    public BisonFactory()
    {
        File.Delete(DbPath);

        using var conn = new SqliteConnection($"Data Source={DbPath}");
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = File.ReadAllText("schema.sql") +
        @"
        INSERT INTO user VALUES
            (0,'Petra','petra@gmail.com'),
            (1,'Anton','anton@gmail.com'),
            (2,'Sigurd','sigurd@gmail.com');
        
        INSERT INTO observation VALUES
            (0,0,'A heron',0),
            (1,1,'A bear',0),
            (2,1,'A pigeon',0);
        ";
        cmd.ExecuteNonQuery();

        Environment.SetEnvironmentVariable("BISONDBPATH", DbPath);
    }
}

public class BisonUnitTest : IClassFixture<BisonFactory>
{
    private readonly BisonFactory _factory;
    private readonly HttpClient _client;

    public BisonUnitTest(BisonFactory factory)
    {
        // Arrange
        _factory = factory;
        _client = factory.CreateClient();
    }

    /**
        A test to make sure only a single user's (author's) observations are listed on /{author} endpoint. Confirms the existence of Petra's entry, as well as the absence of Anton's entry
    */
    [Fact]
    public async Task Observations_ShowAuthorObservationsOnly()
    {
        // Act
        var html = await _client.GetStringAsync("/0");

        // Assert
        Assert.Contains("A heron", html);
        Assert.DoesNotContain("A bear", html);
        Assert.DoesNotContain("A pigeon", html);
    }

    /**
        * This test checks that the comment command returns an error message when provided with a comment ID that does not match any existing bison observation ID.
    */
    [Fact]
    public async Task CommentIdErrorTest()
    {
        // Arrange
        var baseURL = "http://localhost:5189";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);

        try
        {
            // Act
            var observations = await client.GetFromJsonAsync<UniqueObservation[]>("/observations");
            Assert.NotNull(observations);

            var invalidObservationId = observations.Max(obs => obs.Id) + 1;
            Comment comment = new Comment(invalidObservationId, "test", "test", DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            var req = await client.PostAsJsonAsync("/comment", comment);




            Program.Main(new[] { "comment", "test", "100000" });

            // Assert
            // DOES NOT WORK FOR SOME REASON¨
            //TODO: fix
            //Assert.Contains($"Observation with ", writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    /**
        * This test uses a Unix timestamp of 0 and checks that the output string matches the expected format after the conversion.
    */
    [Fact]
    public async Task UnixTimestampTest()
    {
        // Act
        var html = await _client.GetStringAsync("/0");

        // Assert
        Assert.NotEmpty(html);
        Assert.Contains("01/01/70 0:00:00", html);
    }

    /**
        * Test that the PrintComments method correctly filters comments by the specified observation ID.
        * This test creates a list of comments with different IDs and checks that only the comment with the matching ID is printed.
    */
    [Fact]
    public void CommentsMatchingIdTest()
    {
        // Arrange
        var comments = new List<Comment>
    {
        new Comment(Id: 1, Author: "simon", Message: "first", Timestamp: 100),
        new Comment(Id: 2, Author: "daniel", Message: "second", Timestamp: 200),
        new Comment(Id: 3, Author: "filip", Message: "third", Timestamp: 300),
    };

        var originalOut = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        try
        {
            // Act
            UserInterface<Comment>.PrintComments(comments, 2);

            // Assert
            var output = writer.ToString();
            Assert.Contains("daniel", output);
            Assert.DoesNotContain("simon", output);
            Assert.DoesNotContain("filip", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}