using Bison.Core.DTOs;
using Bison.Core.Models;
using Bison.Infrastructure.Repositories;

namespace Bison.Infrastructure.Services;

// Wraps the formally verified Dafny filter (Bison.Core/Dafny/Filter.dfy).
public class FilterService : IFilterService
{
    private const string timestampFormat = "MM/dd/yy H:mm:ss";
    private readonly IPostRepository _repository;

    public FilterService(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ObservationDTO>> GetObservationsByTaxon(string taxonId, int page)
    {
        var root = await _repository.GetTaxonEntity(taxonId);
        if (root is null) return new List<ObservationDTO>();

        // Observations without a taxon cannot match any taxon filter, and the Dafny
        // contract assumes getTaxon() is never null, so leave them out.
        var observations = (await _repository.GetObservationsWithTaxa())
            .Where(o => o.Taxon is not null)
            .ToArray();

        var filtered = Bison.Core.Filtering.__default.FilterObservations(root, observations);

        return filtered
            .Skip((page - 1) * IPostRepository.PageSize)
            .Take(IPostRepository.PageSize)
            .Select(o => new ObservationDTO(
                o.PostId,
                o.Text,
                o.TimeStamp.ToString(timestampFormat),
                o.AuthorId,
                o.Author?.UserName,
                o.ImageId,
                o.Image?.License))
            .ToList();
    }
}
