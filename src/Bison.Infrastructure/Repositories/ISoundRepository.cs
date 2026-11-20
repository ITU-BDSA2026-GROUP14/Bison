using Bison.Core.DTOs;

namespace Bison.Infrastructure.Repositories;

public interface ISoundRepository
{
    Task<SoundDTO> GetTaxonSound(string taxonId);
}