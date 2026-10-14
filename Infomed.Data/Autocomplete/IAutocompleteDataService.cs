using Infomed.Domain.Entities;

namespace Infomed.Data.Autocomplete;

public interface IAutocompleteDataService
{
    Task<IEnumerable<DoctorEntity>> GetExpertsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<CityEntity>> GetAreasAndCitiesAsync(CancellationToken cancellationToken);
}
