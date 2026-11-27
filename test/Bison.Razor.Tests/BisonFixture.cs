using System.Diagnostics;
using System.Net.Http;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Bison.Infrastructure.Data;
using Bison.Core.Models;

namespace Bison.Razor.Tests;

// This class is a fixture that will be used for tests
// it is a fixture class, something xUnit uses to
// be able to set enviroments up before every test.
// this class will be used to run the Bison program, 
// set up the database and shut it down once done. 
// It can also share context between test classes
// source: https://xunit.net/docs/shared-context

public class BisonFixture : IAsyncLifetime
{
    private Process? _app;
    public string BaseUrl { get; } = "http://localhost:5123";
    private readonly StringBuilder _output = new();
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), "bison-test.db");
    private BisonDBContext Context;

    // Starts once iniated 
    public async Task InitializeAsync() {

        // delete test database if it exists
        if (File.Exists(_dbPath)) File.Delete(_dbPath);

        // launch the app with base url
        var psi = new ProcessStartInfo("dotnet",
        $"run --project ../../../../../src/Bison.Razor --no-build --no-launch-profile --urls {BaseUrl}")
        {
            UseShellExecute = false,
            RedirectStandardError = true,
        };

        // Fixes keys and sets the database path
        psi.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";
        psi.Environment["BISONDBPATH"] = _dbPath;
        psi.Environment["Authentication__GitHub__ClientId"] = "test-client-id";
        psi.Environment["Authentication__GitHub__ClientSecret"] = "test-client-secret";

        _app = Process.Start(psi)!;

        // creates the database context
        var options = new DbContextOptionsBuilder<BisonDBContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .EnableSensitiveDataLogging()
            .Options;

        Context = new BisonDBContext(options);
        Context.Database.EnsureCreated();
        Seed(Context);

        // check if its actually running 
        // it might be slow in the beginning 
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
        for (var i = 0; i < 60; i++){
            if (_app.HasExited)
                throw new Exception("App crashed on startup:\n" + await _app.StandardError.ReadToEndAsync());

            try
            {
                var res = await http.GetAsync(BaseUrl);
                if (res.IsSuccessStatusCode) return;
            }
            // dont do anything if excption is thrown and wait
            catch (HttpRequestException) {}
            catch (TaskCanceledException) {}
            await Task.Delay(500);
        }
        // if app didnt startup
        throw new Exception($"App did not respond at {BaseUrl} within 30 seconds. Output:\n{_output}");
    }

    // called once done
    public Task DisposeAsync() {
        if (_app is { HasExited: false }) {
            _app.Kill(entireProcessTree: true);
        } 

        Context.Dispose();
        return Task.CompletedTask;
    }

    // creates the DTOs in the database 
    public void Seed(BisonDBContext context) {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0);

        var hejrer = new Taxon { TaxonId = 1, dwc_TaxonID = "test-hejrer", VernacularName = "Hejrer" };
        var fiskehejre = new Taxon { TaxonId = 2, dwc_TaxonID = "test-fiskehejre", VernacularName = "Fiskehejre", Parent = hejrer };
        var pelikaner = new Taxon { TaxonId = 3, dwc_TaxonID = "test-pelikaner", VernacularName = "Pelikaner" };

        var petra = new Author { Id = "1", UserName = "Petra", Email = "petra@gmail.com" };
        var anton = new Author { Id = "2", UserName = "Anton", Email = "anton@gmail.com" };

        var heron   = new Observation { PostId = 1, Author = petra, Taxon = fiskehejre, Text = "A heron",   TimeStamp = epoch };
        var bear    = new Observation { PostId = 2, Author = anton, Taxon = hejrer,     Text = "A bear",    TimeStamp = epoch };
        var pelican = new Observation { PostId = 3, Author = anton, Taxon = pelikaner,  Text = "A pelican", TimeStamp = epoch };

        var comment1  = new Comment  { PostId = 4, Author = anton, Observation = heron, Text = "Great spot!", TimeStamp = epoch };
        var comment2  = new Comment  { PostId = 5, Author = petra, Observation = heron, Text = "YOOOOOO SHUT YOUR BITCH ASS UP", TimeStamp = epoch };

        var proposal1 = new Proposal { PostId = 6, Author = petra, Observation = bear,    Taxon = pelikaner, Text = "Maybe a pelican?", TimeStamp = epoch };
        var proposal2 = new Proposal { PostId = 7, Author = anton, Observation = pelican, Taxon = pelikaner, Text = "WTFFFF IS THAT HOLY SHITTTTT", TimeStamp = epoch };

        context.Taxons.AddRange(hejrer, fiskehejre, pelikaner);
        context.Authors.AddRange(petra, anton);
        context.Posts.AddRange(heron, bear, pelican, comment1, comment2, proposal1, proposal2);
        context.SaveChanges();
    }
}