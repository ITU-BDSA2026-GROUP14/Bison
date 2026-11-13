using Bison.Core.DTOs;
using Bison.Infrastructure.Repositories;

namespace Bison.Infrastructure.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _repository;

    public PostService(IPostRepository repository)
    {
        _repository = repository;
    }

    public Task<ObservationDTO?> GetObservation(int observationId) => _repository.GetObservation(observationId);
    public Task<List<ObservationDTO>> GetObservations(int page) => _repository.GetObservations(page);
    public Task<List<ObservationDTO>> GetObservationsByAuthor(string id, int page) => _repository.GetObservationsByAuthor(id, page);
    public Task<List<CommentDTO>> GetCommentsForObservation(int observationId, int page) => _repository.GetCommentsForObservation(observationId, page);
    public Task<List<CommentDTO>> GetCommentsByAuthor(string id, int page) => _repository.GetCommentsByAuthor(id, page);
    public Task<List<ProposalDTO>> GetProposals(int page) => _repository.GetProposals(page);
    public Task<List<ProposalDTO>> GetProposalsForObservation(int observationId, int page) => _repository.GetProposalsForObservation(observationId, page);
    public Task<AuthorDTO?> GetAuthor(string id) => _repository.GetAuthor(id);
    public Task<TaxonDTO?> GetTaxon(string taxonId) => _repository.GetTaxon(taxonId);
    public Task<List<TaxonDTO>> GetTaxa() => _repository.GetTaxa();
    public Task<ImageDTO?> GetImage(int imageId) => _repository.GetImage(imageId);
    public Task CreateObservation(string authorEmail, string text, byte[]? picture, string? pictureContentType) =>
    _repository.CreateObservation(authorEmail, text, picture, pictureContentType);
    
    // Creates a comment on an observation for the logged-in user
    public async Task<int> CreateComment(string authorEmail, int observationId, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("A comment must have text", nameof(text));

        var authorId = await GetOrCreateAuthorId(authorEmail);
        return await _repository.CreateComment(authorId, observationId, text.Trim());
    }

    // Creates a proposal for which taxon an observation shows. The explanation text is optional.
    public async Task<int> CreateProposal(string authorEmail, int observationId, string taxonId, string? text)
    {
        if (string.IsNullOrWhiteSpace(taxonId))
            throw new ArgumentException("A proposal must have a taxon", nameof(taxonId));

        var authorId = await GetOrCreateAuthorId(authorEmail);
        return await _repository.CreateProposal(authorId, observationId, taxonId, text?.Trim() ?? "");
    }

    // Finds the Author with this email, or creates one the first time the user posts something
    private async Task<int> GetOrCreateAuthorId(string email)
    {
        var author = await _repository.GetAuthorByEmail(email);
        return author?.Id ?? await _repository.CreateAuthor(email, email);
    }
}
