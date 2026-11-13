// TestDatabase.cs
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Bison.Core.Models;
using Bison.Infrastructure.Data;

namespace Bison.Infrastructure.Tests;

public abstract class TestDatabase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly BisonDBContext Context;

    protected TestDatabase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<BisonDBContext>()
        .UseSqlite(_connection)
        .EnableSensitiveDataLogging()
        .Options;

        Context = new BisonDBContext(options);
        Context.Database.EnsureCreated();
        Seed(Context);
    }

    protected virtual void Seed(BisonDBContext context)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0);

        var hejrer = new Taxon { TaxonId = 1, dwc_TaxonID = "test-hejrer", VernacularName = "Hejrer" };
        var fiskehejre = new Taxon { TaxonId = 2, dwc_TaxonID = "test-fiskehejre", VernacularName = "Fiskehejre", Parent = hejrer };
        var pelikaner = new Taxon { TaxonId = 3, dwc_TaxonID = "test-pelikaner", VernacularName = "Pelikaner" };

        var petra = new Author { AuthorId = 1, Name = "Petra", Email = "petra@gmail.com" };
        var anton = new Author { AuthorId = 2, Name = "Anton", Email = "anton@gmail.com" };

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

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
    }
}