using Bison.Core.DTOs;
using Bison.Infrastructure.Repositories;

namespace Bison.Infrastructure.Services;

public interface IPostService
{
    Task<ObservationDTO?> GetObservation(int observationId);
    Task<List<ObservationDTO>> GetObservations(int page);
    Task<List<ObservationDTO>> GetObservationsByAuthor(string id, int page);
    Task<List<CommentDTO>> GetCommentsForObservation(int observationId, int page);
    Task<List<CommentDTO>> GetCommentsByAuthor(string id, int page);
    Task<List<ProposalDTO>> GetProposals(int page);
    Task<List<ProposalDTO>> GetProposalsForObservation(int observationId, int page);
    Task<AuthorDTO?> GetAuthor(string id);
    Task CreateObservation(string authorEmail, string text, byte[]? picture, string? pictureContentType);
    Task<TaxonDTO?> GetTaxon(string taxonId);
    Task<ImageDTO?> GetImage(int imageId);
}