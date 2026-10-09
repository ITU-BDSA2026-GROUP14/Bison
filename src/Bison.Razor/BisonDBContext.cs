using Bison.Razor.Models;
using Microsoft.EntityFrameworkCore;

namespace Bison.Razor;

public class BisonDBContext : DbContext
{
    public const int pageSize = 32;

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
        .FirstOrDefaultAsync(o => o.Id == observationId);
    }

    // Retrieves all Observations from the database
    public async Task<List<Observation>> GetObservations(int page = 1)
    {
        return await Observations
        .OrderByDescending(o => o.PubDate)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

    // Retrieves all Observations from a specific Author by AuthorId
    public async Task<List<Observation>> GetObservationsByAuthor(int authorId, int page = 1)
    {
        return await Observations
        .OrderByDescending(o => o.PubDate)
        .Where(o => o.Author.Id == authorId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

    // Retrieves all Comments for a specific Observation
    public async Task<List<Comment>> GetCommentsForObservation(int observationId, int page = 1)
    {
        return await Comments
        .OrderByDescending(c => c.PubDate)
        .Where(c => c.Observation.Id == observationId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

    // Retrieves all Comments by a specific Author
    public async Task<List<Comment>> GetCommentsByAuthor(int authorId, int page = 1)
    {
        return await Comments
        .OrderByDescending(c => c.PubDate)
        .Where(c => c.Author.Id == authorId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

    // Retrieves all Proposals from the database
    public async Task<List<Proposal>> GetProposals(int page = 1)
    {
        return await Proposals
        .OrderByDescending(p => p.PubDate)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

    // Retrieves all Proposals for a specific Observation
    public async Task<List<Proposal>> GetProposalsForObservation(int observationId, int page = 1)
    {
        return await Proposals
        .OrderByDescending(p => p.PubDate)
        .Where(p => p.Observation.Id == observationId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

    // Retrieves an Author from its id
    public async Task<Author?> GetAuthor(int authorId)
    {
        return await Authors
        .FirstOrDefaultAsync(a => a.Id == authorId);
    }

    // Retrieves Taxon from its id
    public async Task<Taxon?> GetTaxon(string taxonId)
    {
        return await Taxons
        .FirstOrDefaultAsync(t => t.dwc_TaxonID == taxonId);
    }
}