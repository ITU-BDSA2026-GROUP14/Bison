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
using Bison.Razor;
using Bison.Razor.Models;
using Microsoft.EntityFrameworkCore;

public class BisonFactory : WebApplicationFactory<Program>
{
    private string DbPath { get; } = Path.Combine(Path.GetTempPath(), "bison-test.db");

    public BisonFactory()
    {
        File.Delete(DbPath);

        var options = new DbContextOptionsBuilder<BisonDBContext>()
            .UseSqlite($"Data Source={DbPath}")
            .Options;

        using (var context = new BisonDBContext(options))
        {
            // Let EF Core create the tables from the model, then insert known test data.
            context.Database.EnsureCreated();

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0);
            var taxon = new Taxon { TaxonId = 1, dwc_TaxonID = "test-taxon", VernacularName = "Hejre" };

            var petra = new Author { AuthorId = 1, Name = "Petra", Email = "petra@gmail.com" };
            var anton = new Author { AuthorId = 2, Name = "Anton", Email = "anton@gmail.com" };
            var sigurd = new Author { AuthorId = 3, Name = "Sigurd", Email = "sigurd@gmail.com" };

            var heron = new Observation { PostId = 1, Author = petra, Taxon = taxon, Text = "A heron", TimeStamp = epoch };
            var bear = new Observation { PostId = 2, Author = anton, Taxon = taxon, Text = "A bear", TimeStamp = epoch };
            var pigeon = new Observation { PostId = 3, Author = anton, Taxon = taxon, Text = "A pigeon", TimeStamp = epoch };

            var heronComment = new Comment { PostId = 4, Author = anton, Observation = heron, Text = "Great spot on the heron!", TimeStamp = epoch };
            var bearComment = new Comment { PostId = 5, Author = sigurd, Observation = bear, Text = "Nice bear photo", TimeStamp = epoch };
            var bearProposal = new Proposal { PostId = 6, Author = sigurd, Observation = bear, Taxon = taxon, Text = "Brown bear", TimeStamp = new DateTime(2023, 8, 1, 7, 0, 0) };

            context.Taxons.Add(taxon);
            context.Authors.AddRange(petra, anton, sigurd);
            context.Posts.AddRange(heron, bear, pigeon, heronComment, bearComment, bearProposal);
            context.SaveChanges();
        }
        SqliteConnection.ClearAllPools(); // release the file so the app can open it cleanly

        // The app seeds only when the database is empty, so it keeps this test data.
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
        var html = await _client.GetStringAsync("/ob/2");

        Assert.Contains("A bear", html);
        Assert.Contains("Anton", html);
        Assert.Contains("Nice bear photo", html); // The comment
        Assert.Contains("Brown bear", html); // The proposal
    }

    [Fact]
    public async Task ObPage_WithoutCommentsOrProposals_SaysSo()
    {
        var html = await _client.GetStringAsync("/ob/3");

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

    /**
        A test to make sure only a single user's (author's) observations are listed on /{author} endpoint. Confirms the existence of Petra's entry, as well as the absence of Anton's entry
    */
    [Fact]
    public async Task Observations_ShowAuthorObservationsOnly()
    {
        // Act
        var html = await _client.GetStringAsync("/1");

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
        var html = await _client.GetStringAsync("/ob/1");

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
        var html = await _client.GetStringAsync("/ob/1");

        // Assert
        Assert.NotEmpty(html);
        Assert.Contains("Great spot on the heron!", html);
        Assert.DoesNotContain("Nice bear photo", html);
    }


}