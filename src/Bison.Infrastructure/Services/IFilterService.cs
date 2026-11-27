using Bison.Core.DTOs;

namespace Bison.Infrastructure.Services;

public interface IFilterService
{
    // Observations whose taxon is the given taxon or one of its subtaxa, newest first, one page at a time.
    // An unknown taxonId gives an empty list.
    Task<List<ObservationDTO>> GetObservationsByTaxon(string taxonId, int page);
}
