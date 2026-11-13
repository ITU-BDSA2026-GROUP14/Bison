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
    public Task<ImageDTO?> GetImage(int imageId) => _repository.GetImage(imageId);
    public Task CreateObservation(string authorEmail, string text, byte[]? picture, string? pictureContentType) =>
    _repository.CreateObservation(authorEmail, text, picture, pictureContentType);
    
}