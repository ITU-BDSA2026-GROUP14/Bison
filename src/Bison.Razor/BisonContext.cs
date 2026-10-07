using Microsoft.EntityFrameworkCore;

namespace Bison.Razor;

public class BisonContext : DbContext
{
    public const int pageSize = 32;
    private const string timestampFormat = "MM/dd/yy H:mm:ss";

    public DbSet<Observation> Observations { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Proposal> Proposals { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Taxon> Taxons { get; set; }

    public BisonContext(DbContextOptions<BisonContext> options) : base(options) { }

    // Retrieves all Observations from the database
    public async Task<List<ObservationViewModel>> GetObservations(int page)
    {
        return await Observations
        .OrderByDescending(o => o.PubDate)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(o => new ObservationViewModel(
            o.Author.AuthorId,
            o.Author.Username,
            o.Text,
            o.PubDate.ToString(timestampFormat)
        ))
        .ToListAsync();
    }

    // Retrieves all Observations from a specific Author by AuthorId
    public async Task<List<ObservationViewModel>> GetObservationsFromAuthorId(int authorId, int page)
    {
        return await Observations
        .OrderByDescending(o => o.PubDate)
        .Where(o => o.Author.AuthorId == authorId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(o => new ObservationViewModel(
            o.Author.AuthorId,
            o.Author.Username,
            o.Text,
            o.PubDate.ToString(timestampFormat)
        ))
        .ToListAsync();
    }

    // Retrieve an Observation its id
    public async Task<List<ObservationViewModel>> GetObservationFromId(int observationId, int page)
    {
        return await Observations
        .OrderByDescending(o => o.PubDate)
        .Where(o => o.id == observationId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(o => new ObservationViewModel(
            o.Author.AuthorId,
            o.Author.Username,
            o.Text,
            o.PubDate.ToString(timestampFormat)
        ))
        .ToListAsync();
    }

    // Retrieves all Comments for a specific Observation
    public async Task<List<ObservationViewModel>> GetCommentsFromObservationId(int observationId, int page)
    {
        return await Comments
        .OrderByDescending(c => c.PubDate)
        .Where(c => c.Observation.id == observationId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(c => new ObservationViewModel(
            c.Author.AuthorId,
            c.Author.Username,
            c.Text,
            c.PubDate.ToString(timestampFormat)
        ))
        .ToListAsync();
    }

    // Retrieves all Comments by a specific Author
    public async Task<List<ObservationViewModel>> GetCommentsFromAuthorId(int authorId, int page)
    {
        return await Comments
        .OrderByDescending(c => c.PubDate)
        .Where(c => c.Author.id == authorId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(c => new ObservationViewModel(
            c.Author.AuthorId,
            c.Author.Username,
            c.Text,
            c.PubDate.ToString(timestampFormat)
        ))
        .ToListAsync();
    }

    // Retrieves all Proposals from the database
    public async Task<List<ObservationViewModel>> GetProposals(int page)
    {
        return await Proposals
        .OrderByDescending(p => p.PubDate)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ObservationViewModel(
            p.Author.AuthorId,
            p.Author.Username,
            p.Text,
            p.PubDate.ToString(timestampFormat)
        ))
        .ToListAsync();
    }

    // Retrieves all Proposals for a specific Observation
    public async Task<List<ObservationViewModel>> GetProposalsFromObservationId(int observationId, int page)
    {
        return await Proposals
        .OrderByDescending(p => p.PubDate)
        .Where(p => p.Observation.id == observationId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ObservationViewModel(
            p.Author.AuthorId,
            p.Author.Username,
            p.Text,
            p.PubDate.ToString(timestampFormat)
        ))
        .ToListAsync();
    }

}