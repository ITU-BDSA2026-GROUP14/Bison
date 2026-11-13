using Bison.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Bison.Core.DTOs;
using Bison.Core.Models;   

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
            obs.Author.AuthorId,
            obs.Author.Name,
            obs.ImageId
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
            o.Author.AuthorId,
            o.Author.Name,
            o.ImageId
        ))
        .ToList();
    }

    // Retrieves all Observations from a specific Author by AuthorId
    public async Task<List<ObservationDTO>> GetObservationsByAuthor(int authorId, int page)
    {
        var obs = await _context.Observations
        .Include(o => o.Author)
        .OrderByDescending(o => o.TimeStamp)
        .Where(o => o.Author.AuthorId == authorId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
        
        return obs.Select(o => new ObservationDTO(
            o.PostId,
            o.Text,
            o.TimeStamp.ToString(timestampFormat),
            o.Author.AuthorId,
            o.Author.Name,
            o.ImageId
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
            c.Author.AuthorId,
            c.Author.Name
        ))
        .ToList();
    }

    // Retrieves all Comments by a specific Author
    public async Task<List<CommentDTO>> GetCommentsByAuthor(int authorId, int page)
    {
        var comments = await _context.Comments
        .Include(o => o.Author)
        .OrderByDescending(c => c.TimeStamp)
        .Where(c => c.Author.AuthorId == authorId)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        return comments.Select(c => new CommentDTO(
            c.PostId,
            c.TimeStamp.ToString(timestampFormat),
            c.Text,
            c.Author.AuthorId,
            c.Author.Name
        ))
        .ToList();
    }

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
            p.Author.AuthorId,
            p.Author.Name,
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
            p.Author.AuthorId,
            p.Author.Name,
            p.Taxon.dwc_TaxonID
        ))
        .ToList();
    }

    // Retrieves an Author from its id
    public async Task<AuthorDTO?> GetAuthor(int authorId)
    {
        var author = await _context.Authors
        .FirstOrDefaultAsync(a => a.AuthorId == authorId);

        if (author is null) return null;

        return new AuthorDTO(
            author.AuthorId,
            author.Name,
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

    public async Task CreateObservation(string authorEmail, string text, byte[]? picture, string? pictureContentType)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(a => a.Email == authorEmail);
        if (author is null)
        {
            author = new Author {Name = authorEmail, Email = authorEmail};
            _context.Authors.Add(author);
        }
        var observation = new Observation
        {
            Text = text,
            TimeStamp = DateTime.Now,
            Author = author
        };
        if (picture != null)
        {
            observation.Image = new Image {Data = picture, ContentType = pictureContentType!};
        }
        _context.Observations.Add(observation);
        await _context.SaveChangesAsync();
    }

        public async Task<ImageDTO?> GetImage(int imageId)
    {
        var image = await _context.Images.FindAsync(imageId);
        if (image is null) return null;
        return new ImageDTO(image.ImageId, image.Data, image.ContentType);
    }
}
    
    

