public interface IPostRepository
{
    public const int PageSize = 32;

    Task<List<ObservationDTO>> GetObservations(int page);
    Task<List<ObservationDTO>> GetObservationsByAuthor(int authorId, int page);
    Task<ObservationDTO?> GetObservation(int observationId);
    Task<List<CommentDTO>> GetCommentsForObservation(int observationId);
    Task<List<ProposalDTO>> GetProposalsForObservation(int observationId);
}