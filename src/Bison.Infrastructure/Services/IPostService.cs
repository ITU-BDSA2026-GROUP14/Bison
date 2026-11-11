using Bison.Core.DTOs;
using Bison.Infrastructure.Repositories;

namespace Bison.Infrastructure.Services;

public interface IPostService
{
    Task<ObservationDTO?> GetObservation(int observationId);
    Task<List<ObservationDTO>> GetObservations(int page);
    Task<List<ObservationDTO>> GetObservationsByAuthor(int authorId, int page);
    Task<List<CommentDTO>> GetCommentsForObservation(int observationId, int page);
    Task<List<CommentDTO>> GetCommentsByAuthor(int authorId, int page);
    Task<List<ProposalDTO>> GetProposals(int page);
    Task<List<ProposalDTO>> GetProposalsForObservation(int observationId, int page);
    Task<AuthorDTO?> GetAuthor(int authorId);
    Task<TaxonDTO?> GetTaxon(string taxonId);
}