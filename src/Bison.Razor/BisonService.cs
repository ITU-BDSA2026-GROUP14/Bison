using Bison.Razor;

public record ObservationViewModel(int Author_id, string AuthorEmail, string Author, string Message, string Timestamp);

public interface IObservationService
{
    public Task<List<ObservationViewModel>> GetObservations(int page);
    public Task<List<ObservationViewModel>> GetObservationsFromAuthorId(int author_id, int page);
    public Task<List<ObservationViewModel>> GetObservationFromId(int observation_id, int page);
    public Task<List<ObservationViewModel>> GetCommentsFromObservationId(int observation_id, int page);
    public Task<List<ObservationViewModel>> GetCommentsFromAuthorId(int author_id, int page);
    public Task<List<ObservationViewModel>> GetProposalsFromId(int id, int page);
    public Task<List<ObservationViewModel>> GetProposals(int page);
}

public class ObservationService : IObservationService
{
    private readonly BisonDBContext _bisonDBContext;

    public ObservationService(BisonDBContext bisonDBContext)
    {
        _bisonDBContext = bisonDBContext;
    }

    public Task<List<ObservationViewModel>> GetObservations(int page)
    {
        return _bisonDBContext.GetObservations(page);
    }

    public Task<List<ObservationViewModel>> GetObservationsFromAuthorId(int author_id, int page)
    {
        return _bisonDBContext.GetObservationsFromAuthorId(author_id, page);
    }

    public Task<List<ObservationViewModel>> GetObservationFromId(int observation_id, int page)
    {
        return _bisonDBContext.GetObservationFromId(observation_id, page);
    }
    public Task<List<ObservationViewModel>> GetCommentsFromObservationId(int observation_id, int page)
    {
        return _bisonDBContext.GetCommentsFromObservationId(observation_id, page);
    }
    public Task<List<ObservationViewModel>> GetCommentsFromAuthorId(int author_id, int page)
    {
        return _bisonDBContext.GetCommentsFromAuthorId(author_id, page);
    }

    public Task<List<ObservationViewModel>> GetProposalsFromId(int id, int page)
    {
        return _bisonDBContext.GetProposalsFromObservationId(id, page);
    }

    public Task<List<ObservationViewModel>> GetProposals(int page)
    {
        return _bisonDBContext.GetProposals(page);
    }
}
