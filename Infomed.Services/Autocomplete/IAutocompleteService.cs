using Infomed.Domain.Dto;

namespace Infomed.Services.Autocomplete;

public interface IAutocompleteService
{
    Task<IEnumerable<AutocompleteOptionDto>> SearchExpertsAsync(string searchTerm, CancellationToken cancellationToken);
    Task<IEnumerable<AutocompleteOptionDto>> SearchAreasAndCitiesAsync(string searchTerm, CancellationToken cancellationToken);
}
