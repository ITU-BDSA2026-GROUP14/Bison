public class PostService : IPostService 
{
    private readonly IPostRepository _repository;

    public PostService(IPostRepository repository)
    {
        _repository = repository;
    }

    public Task<ObservationDTO?> GetObservation(int observationId) => _repository.GetObservation(observationId);
    public Task<List<ObservationDTO>> GetObservations(int page) => _repository.GetObservations(page);
    public Task<List<ObservationDTO>> GetObservationsByAuthor(int authorId, int page) => _repository.GetObservationsByAuthor(authorId,page);
    public Task<List<CommentDTO>> GetCommentsForObservation(int observationId, int page) => _repository.GetCommentsForObservation(observationId,page);
    public Task<List<CommentDTO>> GetCommentsByAuthor(int authorId, int page) => _repository.GetCommentsByAuthor(authorId,page);
    public Task<List<ProposalDTO>> GetProposals(int page) => _repository.GetProposals(page);
    public Task<List<ProposalDTO>> GetProposalsForObservation(int observationId, int page) => _repository.GetProposalsForObservation(observationId,page);
    public Task<AuthorDTO?> GetAuthor(int authorId) => _repository.GetAuthor(authorId);
    public Task<TaxonDTO?> GetTaxon(string taxonId) => _repository.GetTaxon(taxonId);
}