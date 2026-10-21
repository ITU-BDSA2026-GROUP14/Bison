using Bison.Razor;

public class PostRepository : IPostRepository
{
    private const string timestampFormat = "MM/dd/yy H:mm:ss";
    private readonly BisonDBContext _context;

    public PostRepository(BisonDBContext context)
    {
        _context = context;
    }

    public async Task<ObservationDTO?> GetObservation(int observationId)
    {
        var obs = await _context.GetObservation(observationId);
        if (obs is null) return null;
        return new ObservationDTO(
            obs.PostId,
            obs.Text,
            obs.TimeStamp.ToString(timestampFormat),
            obs.Author.AuthorId,
            obs.Author.Name
            );
    }

    public async Task<List<ObservationDTO>> GetObservations(int page)
    {
        var obs = await _context.GetObservations(page);
        return obs.Select(o => new ObservationDTO(
            o.PostId,
            o.Text,
            o.TimeStamp.ToString(timestampFormat),
            o.Author.AuthorId,
            o.Author.Name
        ))
        .ToList();
    }

    public async Task<List<ObservationDTO>> GetObservationsByAuthor(int authorId, int page)
    {
        var comments = await _context.GetObservationsByAuthor(authorId, page);
        return comments.Select(o => new ObservationDTO(
            o.PostId,
            o.Text,
            o.TimeStamp.ToString(timestampFormat),
            o.Author.AuthorId,
            o.Author.Name
        ))
        .ToList();
    }

    public async Task<List<CommentDTO>> GetCommentsForObservation(int observationId, int page)
    {
        var comments = await _context.GetCommentsForObservation(observationId, page);
        return comments.Select(c => new CommentDTO(
            c.PostId,
            c.TimeStamp.ToString(timestampFormat),
            c.Text,
            c.Author.AuthorId,
            c.Author.Name
        ))
        .ToList();
    }

    public async Task<List<CommentDTO>> GetCommentsByAuthor(int authorId, int page)
    {
        var comments = await _context.GetCommentsByAuthor(authorId, page);
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
        var proposals = await _context.GetProposals(page);
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

    public async Task<List<ProposalDTO>> GetProposalsForObservation(int observationId, int page)
    {
        var proposals = await _context.GetProposalsForObservation(observationId, page);
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

    public async Task<AuthorDTO?> GetAuthor(int authorId)
    {
        var author = await _context.GetAuthor(authorId);
        if (author is null) return null;
        return new AuthorDTO(
            author.AuthorId,
            author.Name,
            author.Email
        );
    }

    public async Task<TaxonDTO?> GetTaxon(string taxonId)
    {
        var taxon = await _context.GetTaxon(taxonId);
        if (taxon is null) return null;
        return new TaxonDTO(
            taxon.dwc_TaxonID,
            taxon.VernacularName,
            taxon.Parent?.dwc_TaxonID
        );
    }
}