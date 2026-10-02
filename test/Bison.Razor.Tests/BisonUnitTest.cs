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
        
        INSERT INTO comment (observation_id, author_id, text, pub_date) VALUES
            (0, 1, 'Great spot on the heron!', 0),
            (1, 2, 'Nice bear photo', 0);

        INSERT INTO proposal VALUES (1, 1, 2, 'Brown bear', 1690897000);

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

    [Fact]
    public async Task GetAllObservations()
    {
        // Act
        var html = await _client.GetStringAsync("/");

        // Assert
        Assert.NotEmpty(html);
        Assert.Contains("A heron", html);
        Assert.Contains("A bear", html);
        Assert.Contains("A pigeon", html);
    }


    [Fact]
    public async Task ObPage_ShowsObservationWithCommentsAndProposal()
    {
        var html = await _client.GetStringAsync("/ob/1");

        Assert.Contains("A bear", html);
        Assert.Contains("Anton", html);
        Assert.Contains("Nice bear photo", html); // The comment
        Assert.Contains("Brown bear", html); // The proposal
    }

    [Fact]
    public async Task ObPage_WithoutCommentsOrProposals_SaysSo()
    {
        var html = await _client.GetStringAsync("/ob/2");

        Assert.Contains("A pigeon", html);
        Assert.Contains("There are no comments so far", html);
        Assert.Contains("There are no proposals so far", html);
    }

    [Fact]
    public async Task ObPage_WithoutId_ShowsAllObservations()
    {
        // /ob/ redirects to /obs; the client follows the redirect automatically
        var html = await _client.GetStringAsync("/ob/");

        // Assert
        Assert.NotEmpty(html);
        Assert.Contains("Public Timeline", html);
        Assert.Contains("A heron", html);
        Assert.Contains("A bear", html);
        Assert.Contains("A pigeon", html);
    }


    // public class ObPageApiTests : IDisposable
    // {
    //     private readonly string _dbPath;
    //     private readonly WebApplicationFactory<Program> _factory;
    //     private readonly HttpClient _client;

    //     public ObPageApiTests()
    //     {
    //         // Create a fresh test database: the real schema + some known data
    //         _dbPath = Path.Combine(Path.GetTempPath(), $"bison-test-{Guid.NewGuid()}.db");
    //         using (var connection = new SqLiteConnection($"Data Source={_dbPath}"))
    //         {
    //             connection.Open();
    //             using var command = connection.CreateCommand();
    //             command.CommandText = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "schema.sql")) + @"
    //             INSERT INTO user VALUES (1, 'Peter', 'peter@itu.dk');
    //             INSERT INTO user VALUES (2, 'Petra', 'petra@itu.dk');
    //             INSERT INTO observation VALUES (1, 1, 'A big gray bird in a pond at DR byen', 1690892208);
    //             INSERT INTO observation VALUES (2, 2, 'A heron', 1690895308);
    //             INSERT INTO comment VALUES (1, 1, 2, 'Probably a heron', 1690896000);
    //             INSERT INTO proposal VALUES (1, 1, 2, 'Fiskehejre', 1690897000);";
    //             command.ExecuteNonQuery();
    //         }
    //         // Point the app at the test database (the app reads BISONDBPATH at startup)
    //         Environment.SetEnvironmentVariable("BISONDBPATH", _dbPath);

    //         // Start the app in memory and get a client that sends requests to it
    //         _factory = new WebApplicationFactory<Program>();
    //         _client = _factory.CreateClient();
    //     }

    //     [Fact]
    //     public async Task ObPage_ShowsObservationWithCommentsAndProposal()
    //     {
    //         var html = await_client.GetStringAsync("/ob/1");

    //         Assert.Contains("A big gray bird in a pond at DR byen", html);
    //         Assert.Contains("Peter", html);
    //         Assert.Contains("Probably a heron", html); // The comment
    //         Assert.Contains("Fiskehejre", html); // The proposal
    //     }

    //     [Fact]
    //     public async Task ObPage_WithoutCommentsOrProposals_SaysSo()
    //     {
    //         var html = await_client.GetStringAsync("/ob/2");

    //         Assert.Contains("A heron", html);
    //         Assert.Contains("There are no comments so far", html);
    //         Assert.Contains("There are no proposals so far", html);
    //     }

    //     [Fact]
    //     public async Task ObPage_WithoutId_ShowsAllObservations()
    //     {
    //         // /ob/ redirects to /obs; the client follows the redirect automatically
    //         var html = await_client.GetStringAsync("/ob/");

    //         AssertContains("Public Timeline", html);
    //         AssertContains("A big gray bird in a pond at DR byen", html);
    //         AssertContains("A heron", html);
    //     }

    //     public void Dispose()
    //     {
    //         _client.Dispose();
    //         _factory.Dispose();
    //         SqLiteConnection.ClearAllPools();
    //         File.Delete(_dbPath);
    //     }
    // }

    /**
        A test to make sure only a single user's (author's) observations are listed on /{author} endpoint. Confirms the existence of Petra's entry, as well as the absence of Anton's entry
    */
    [Fact]
    public async Task Observations_ShowAuthorObservationsOnly()
    {
        // Act
        var html = await _client.GetStringAsync("/ob/0");

        // Assert
        Assert.NotEmpty(html);
        Assert.Contains("A heron", html);
        Assert.DoesNotContain("A bear", html);
        Assert.DoesNotContain("A pigeon", html);
    }

    /**
        This test uses a Unix timestamp of 0 and checks that the output string matches the expected format after the conversion.
    */
    [Fact]
    public async Task UnixTimestampTest()
    {
        // Act
        var html = await _client.GetStringAsync("/ob/0");

        // Assert
        Assert.NotEmpty(html);
        Assert.Contains("Great spot on the heron!", html);
        Assert.Contains("01/01/70 0:00:00", html);
    }

    /**
        Test that the PrintComments method correctly filters comments by the specified observation ID.
        This test creates a list of comments with different IDs and checks that only the comment with the matching ID is printed.
    */
    [Fact]
    public async Task CommentsMatchingIdTest()
    {
        // Act
        var html = await _client.GetStringAsync("/ob/0");

        // Assert
        Assert.NotEmpty(html);
        Assert.Contains("Great spot on the heron!", html);
        Assert.DoesNotContain("Nice bear photo", html);
    }


}