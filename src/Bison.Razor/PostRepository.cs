using Bison.Razor;
using Microsoft.EntityFrameworkCore;

public class PostRepository : IPostRepository
{
    private readonly BisonDBContext _context;

    public PostRepository(BisonDBContext context)
    {
        _context = context;
    }

    public async Task<List<ObservationDTO>> GetObservations(int page)
    {
        return _context.GetObservations(page);
        // var rows = await _context.Posts.OfType<Observation>()
        //     .OrderByDescending(o => o.TimeStamp).ThenByDescending(o => o.PostId)
        //     .Skip((Math.Max(page, 1) - 1) * IPostRepository.PageSize)
        //     .Take(IPostRepository.PageSize)
        //     .Select(o => new { o.PostId, o.Author.AuthorId, o.Author.Name, o.Text, o.TimeStamp, Taxon = o.Taxon.VernacularName })
        //     .ToListAsync();

        // return rows
        //     .Select(r => new ObservationDTO(r.PostId, r.AuthorId, r.Name, r.Text, Format(r.TimeStamp), r.Taxon))
        //     .ToList();
    }

    public async Task<List<ObservationDTO>> GetObservationsByAuthor(int authorId, int page)
    {
        var rows = await _context.Posts.OfType<Observation>()
            .Where(o => o.Author.AuthorId == authorId)
            .OrderByDescending(o => o.TimeStamp).ThenByDescending(o => o.PostId)
            .Skip((Math.Max(page, 1) - 1) * IPostRepository.PageSize)
            .Take(IPostRepository.PageSize)
            .Select(o => new { o.PostId, o.Author.AuthorId, o.Author.Name, o.Text, o.TimeStamp, Taxon = o.Taxon.VernacularName })
            .ToListAsync();

        return rows
            .Select(r => new ObservationDTO(r.PostId, r.AuthorId, r.Name, r.Text, Format(r.TimeStamp), r.Taxon))
            .ToList();
    }

    public async Task<ObservationDTO?> GetObservation(int observationId)
    {
        var r = await _context.Posts.OfType<Observation>()
            .Where(o => o.PostId == observationId)
            .Select(o => new { o.PostId, o.Author.AuthorId, o.Author.Name, o.Text, o.TimeStamp, Taxon = o.Taxon.VernacularName })
            .FirstOrDefaultAsync();

        return r is null
            ? null
            : new ObservationDTO(r.PostId, r.AuthorId, r.Name, r.Text, Format(r.TimeStamp), r.Taxon);
    }

    public async Task<List<CommentDTO>> GetCommentsForObservation(int observationId)
    {
        var rows = await _context.Posts.OfType<Comment>()
            .Where(c => c.Observation.PostId == observationId)
            .OrderByDescending(c => c.TimeStamp).ThenByDescending(c => c.PostId)
            .Select(c => new { c.PostId, c.Author.AuthorId, c.Author.Name, c.Text, c.TimeStamp })
            .ToListAsync();

        return rows
            .Select(r => new CommentDTO(r.PostId, r.AuthorId, r.Name, r.Text, Format(r.TimeStamp)))
            .ToList();
    }

    public async Task<List<ProposalDTO>> GetProposalsForObservation(int observationId)
    {
        var rows = await _context.Posts.OfType<Proposal>()
            .Where(p => p.Observation.PostId == observationId)
            .OrderByDescending(p => p.TimeStamp).ThenByDescending(p => p.PostId)
            .Select(p => new { p.PostId, p.Author.AuthorId, p.Author.Name, p.Text, p.TimeStamp, Taxon = p.Taxon.VernacularName })
            .ToListAsync();

        return rows
            .Select(r => new ProposalDTO(r.PostId, r.AuthorId, r.Name, r.Text, Format(r.TimeStamp), r.Taxon))
            .ToList();
    }

    // DateTime is not a predefined type, so DTOs carry the timestamp as a string.
    private static string Format(DateTime timeStamp) => timeStamp.ToString("MM/dd/yy H:mm:ss");
}