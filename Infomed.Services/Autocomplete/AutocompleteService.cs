using AutoMapper;
using Infomed.Data.Autocomplete;
using Infomed.Domain.Dto;

namespace Infomed.Services.Autocomplete;

// Logger not implemented on purpose

public class AutocompleteService(IAutocompleteDataService autocompleteDataService, IMapper mapper) : IAutocompleteService
{
    public Task<IEnumerable<AutocompleteOptionDto>> SearchExpertsAsync(string searchTerm, CancellationToken cancellationToken)
    {
        return SearchAsync(searchTerm, autocompleteDataService.GetExpertsAsync, cancellationToken);
    }

    public Task<IEnumerable<AutocompleteOptionDto>> SearchAreasAndCitiesAsync(string searchTerm, CancellationToken cancellationToken)
    {
        return SearchAsync(searchTerm, autocompleteDataService.GetAreasAndCitiesAsync, cancellationToken);
    }

    private async Task<IEnumerable<AutocompleteOptionDto>> SearchAsync<TEntity>(
        string searchTerm,
        Func<CancellationToken,
        Task<IEnumerable<TEntity>>> loadEntities,
        CancellationToken cancellationToken)
    {
        var entities = await loadEntities(cancellationToken);
        var options = mapper.Map<IEnumerable<AutocompleteOptionDto>>(entities);
        var term = searchTerm.Trim();

        return options
            .Where(option => option.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }
}
