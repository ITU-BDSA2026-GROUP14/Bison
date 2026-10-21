using Bison.Razor.Models;
using Microsoft.EntityFrameworkCore;

namespace Bison.Razor;

public class BisonDBContext : DbContext
{
    public const int pageSize = 32;

    public DbSet<Post> Posts { get; set; }
    public DbSet<Observation> Observations { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Proposal> Proposals { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Taxon> Taxons { get; set; }

    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options) { }

    // Retrieve an Observation its id
    public async Task<Observation?> GetObservation(int observationId)
    {
        return await Observations
        .Include(o => o.Author)
        .FirstOrDefaultAsync(o => o.PostId == observationId);
    }

    // Retrieves all Observations from the database
    public async Task<List<Observation>> GetObservations(int page = 1)
    {
        return await Observations
        .Include(o => o.Author)
        .OrderByDescending(o => o.TimeStamp)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

    // Retrieves all Observations from a specific Author by AuthorId
    public async Task<List<Observation>> GetObservationsByAuthor(int authorId, int page = 1)
    {
        return await Observations
        .Include(o => o.Author)
        .OrderByDescending(o => o.TimeStamp)
        .Where(o => o.Author.AuthorId == authorId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

    // Retrieves all Comments for a specific Observation
    public async Task<List<Comment>> GetCommentsForObservation(int observationId, int page = 1)
    {
        return await Comments
        .Include(o => o.Author)
        .OrderByDescending(c => c.TimeStamp)
        .Where(c => c.Observation.PostId == observationId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

    // Retrieves all Comments by a specific Author
    public async Task<List<Comment>> GetCommentsByAuthor(int authorId, int page = 1)
    {
        return await Comments
        .Include(o => o.Author)
        .OrderByDescending(c => c.TimeStamp)
        .Where(c => c.Author.AuthorId == authorId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

    // Retrieves all Proposals from the database
    public async Task<List<Proposal>> GetProposals(int page = 1)
    {
        return await Proposals
        .Include(o => o.Author)
        .OrderByDescending(p => p.TimeStamp)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

    // Retrieves all Proposals for a specific Observation
    public async Task<List<Proposal>> GetProposalsForObservation(int observationId, int page = 1)
    {
        return await Proposals
        .Include(o => o.Author)
        .OrderByDescending(p => p.TimeStamp)
        .Where(p => p.Observation.PostId == observationId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

    // Retrieves an Author from its id
    public async Task<Author?> GetAuthor(int authorId)
    {
        return await Authors
        .FirstOrDefaultAsync(a => a.AuthorId == authorId);
    }

    // Retrieves Taxon from its id
    public async Task<Taxon?> GetTaxon(string taxonId)
    {
        return await Taxons
        .FirstOrDefaultAsync(t => t.dwc_TaxonID == taxonId);
    }
}