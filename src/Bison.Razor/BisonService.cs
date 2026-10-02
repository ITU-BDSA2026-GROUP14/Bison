public record ObservationViewModel(int Author_id, string Author, string Message, string Timestamp);

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsFromAuthorId(int author_id, int page);
    public List<ObservationViewModel> GetObservationFromObservationId(int observation_id);
    public List<ObservationViewModel> GetCommentsFromObservationId(int observation_id);
    public List<ObservationViewModel> GetCommentsFromAuthorId(int author_id);
    public List<ObservationViewModel> GetProposalsFromId(int id);
    public List<ObservationViewModel> GetProposals();
}

public class ObservationService : IObservationService
{
    private readonly DBFacade _dbFacade;

    public ObservationService(DBFacade dbFacade)
    {
        _dbFacade = dbFacade;
    }

    public List<ObservationViewModel> GetObservations(int page)
    {
        return _dbFacade.GetObservations(page);
    }

    public List<ObservationViewModel> GetObservationsFromAuthorId(int author_id, int page)
    {
        return _dbFacade.GetObservationsFromAuthorId(author_id, page);
    }

    public List<ObservationViewModel> GetObservationFromObservationId(int observation_id)
    {
        return _dbFacade.GetObservationsFromObservationId(observation_id);
    }
    public List<ObservationViewModel> GetCommentsFromObservationId(int observation_id)
    {
        return _dbFacade.GetCommentsFromObservationId(observation_id);
    }
    public List<ObservationViewModel> GetCommentsFromAuthorId(int author_id)
    {
        return _dbFacade.GetCommentsFromAuthorId(author_id);
    }

    public List<ObservationViewModel> GetProposalsFromId(int id)
    {
        return _dbFacade.GetProposalsFromId(id);
    }

    public List<ObservationViewModel> GetProposals()
    {
        return _dbFacade.GetProposals();
    }
}
