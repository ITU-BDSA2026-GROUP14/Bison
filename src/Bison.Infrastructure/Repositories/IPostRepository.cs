using Microsoft.EntityFrameworkCore;
using Bison.Core.DTOs;

namespace Bison.Infrastructure.Repositories;

public interface IPostRepository
{
    public const int PageSize = 32;

    Task<ObservationDTO?> GetObservation(int observationId);
    Task<List<ObservationDTO>> GetObservations(int page);
    Task<List<ObservationDTO>> GetObservationsByAuthor(int authorId, int page);
    Task<List<CommentDTO>> GetCommentsForObservation(int observationId, int page);
    Task<List<CommentDTO>> GetCommentsByAuthor(int observationId, int page);
    Task<List<ProposalDTO>> GetProposals(int page);
    Task<List<ProposalDTO>> GetProposalsForObservation(int observationId, int page);
    Task<AuthorDTO?> GetAuthor(int authorId);
    Task<TaxonDTO?> GetTaxon(string taxonId);

    Task<ImageDTO?> GetImage(int imageId);   

    Task CreateObservation(string authorEmail, string text, byte[]? picture, string? pictureContentType);
}