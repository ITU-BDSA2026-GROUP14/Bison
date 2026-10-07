using Bison.Razor;

public record ObservationViewModel(int Author_id, string Author, string Message, string Timestamp);

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
    private readonly BisonContext _bisonContext;

    public ObservationService(BisonContext bisonContext)
    {
        _bisonContext = bisonContext;
    }

    public Task<List<ObservationViewModel>> GetObservations(int page)
    {
        return _bisonContext.GetObservations(page);
    }

    public Task<List<ObservationViewModel>> GetObservationsFromAuthorId(int author_id, int page)
    {
        return _bisonContext.GetObservationsFromAuthorId(author_id, page);
    }

    public Task<List<ObservationViewModel>> GetObservationFromId(int observation_id, int page)
    {
        return _bisonContext.GetObservationFromId(observation_id, page);
    }
    public Task<List<ObservationViewModel>> GetCommentsFromObservationId(int observation_id, int page)
    {
        return _bisonContext.GetCommentsFromObservationId(observation_id, page);
    }
    public Task<List<ObservationViewModel>> GetCommentsFromAuthorId(int author_id, int page)
    {
        return _bisonContext.GetCommentsFromAuthorId(author_id, page);
    }

    public Task<List<ObservationViewModel>> GetProposalsFromId(int id, int page)
    {
        return _bisonContext.GetProposalsFromObservationId(id, page);
    }

    public Task<List<ObservationViewModel>> GetProposals(int page)
    {
        return _bisonContext.GetProposals(page);
    }
}
