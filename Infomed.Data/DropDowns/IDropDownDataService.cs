using Infomed.Domain.Dto;

namespace Infomed.Data.DropDowns;

public interface IDropDownDataService
{
    Task<IEnumerable<AutocompleteOptionDto>> GetLanguagesAsync(CancellationToken cancellationToken);
}
