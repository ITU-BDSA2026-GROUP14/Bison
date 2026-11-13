using Bison.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Bison.Core.DTOs;

namespace Bison.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private const string timestampFormat = "MM/dd/yy H:mm:ss";
    private readonly BisonDBContext _context;
    private const int pageSize = 32;

    public PostRepository(BisonDBContext context)
    {
        _context = context;
    }

    // Retrieve an Observation from its id
    public async Task<ObservationDTO?> GetObservation(int observationId)
    {
        var obs = await _context.Observations
        .Include(o => o.Author)
        .FirstOrDefaultAsync(o => o.PostId == observationId);

        if (obs is null) return null;

        return new ObservationDTO(
            obs.PostId,
            obs.Text,
            obs.TimeStamp.ToString(timestampFormat),
            obs.Author.Id,
            obs.Author.UserName
        );
    }

    // Retrieves all Observations from the database
    public async Task<List<ObservationDTO>> GetObservations(int page)
    {
        var obs = await _context.Observations
        .Include(o => o.Author)
        .OrderByDescending(o => o.TimeStamp)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return obs.Select(o => new ObservationDTO(
            o.PostId,
            o.Text,
            o.TimeStamp.ToString(timestampFormat),
            o.Author.Id,
            o.Author.UserName
        ))
        .ToList();
    }

    // Retrieves all Observations from a specific Author by Id
    public async Task<List<ObservationDTO>> GetObservationsByAuthor(string id, int page)
    {
        var obs = await _context.Observations
        .Include(o => o.Author)
        .OrderByDescending(o => o.TimeStamp)
        .Where(o => o.Author.Id == id)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return obs.Select(o => new ObservationDTO(
            o.PostId,
            o.Text,
            o.TimeStamp.ToString(timestampFormat),
            o.Author.Id,
            o.Author.UserName
        ))
        .ToList();
    }

    // Retrieves all Comments for a specific Observation
    public async Task<List<CommentDTO>> GetCommentsForObservation(int observationId, int page)
    {
        var comments = await _context.Comments
        .Include(o => o.Author)
        .OrderByDescending(c => c.TimeStamp)
        .Where(c => c.Observation.PostId == observationId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return comments.Select(c => new CommentDTO(
            c.PostId,
            c.TimeStamp.ToString(timestampFormat),
            c.Text,
            c.Author.Id,
            c.Author.UserName
        ))
        .ToList();
    }

    // Retrieves all Comments by a specific Author
    public async Task<List<CommentDTO>> GetCommentsByAuthor(string id, int page)
    {
        var comments = await _context.Comments
        .Include(o => o.Author)
        .OrderByDescending(c => c.TimeStamp)
        .Where(c => c.Author.Id == id)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return comments.Select(c => new CommentDTO(
            c.PostId,
            c.TimeStamp.ToString(timestampFormat),
            c.Text,
            c.Author.Id,
            c.Author.UserName
        ))
        .ToList();
    }

    // Retrieves all Proposals from the database
    public async Task<List<ProposalDTO>> GetProposals(int page)
    {
        var proposals = await _context.Proposals
        .Include(o => o.Author)
        .Include(p => p.Taxon)
        .OrderByDescending(p => p.TimeStamp)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return proposals.Select(p => new ProposalDTO(
            p.PostId,
            p.Text,
            p.TimeStamp.ToString(timestampFormat),
            p.Author.Id,
            p.Author.UserName,
            p.Taxon.dwc_TaxonID
        ))
        .ToList();
    }

    // Retrieves all Proposals for a specific Observation
    public async Task<List<ProposalDTO>> GetProposalsForObservation(int observationId, int page)
    {
        var proposals = await _context.Proposals
        .Include(o => o.Author)
        .Include(p => p.Taxon)
        .OrderByDescending(p => p.TimeStamp)
        .Where(p => p.Observation.PostId == observationId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return proposals.Select(p => new ProposalDTO(
            p.PostId,
            p.Text,
            p.TimeStamp.ToString(timestampFormat),
            p.Author.Id,
            p.Author.UserName,
            p.Taxon.dwc_TaxonID
        ))
        .ToList();
    }

    // Retrieves an Author from its id
    public async Task<AuthorDTO?> GetAuthor(string id)
    {
        var author = await _context.Authors
        .FirstOrDefaultAsync(a => a.Id == id);

        if (author is null) return null;

        return new AuthorDTO(
            author.Id,
            author.UserName,
            author.Email
        );
    }

    // Retrieves Taxon from its id
    public async Task<TaxonDTO?> GetTaxon(string taxonId)
    {
        var taxon = await _context.Taxons
        .Include(t => t.Parent)
        .FirstOrDefaultAsync(t => t.dwc_TaxonID == taxonId);
        if (taxon is null) return null;
        return new TaxonDTO(
            taxon.dwc_TaxonID,
            taxon.VernacularName,
            taxon.Parent?.dwc_TaxonID
        );
    }
}