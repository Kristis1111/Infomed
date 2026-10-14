using Infomed.Domain.Dto;

namespace Infomed.Services.DropDowns;

public interface IDropDownService
{
    Task<IEnumerable<AutocompleteOptionDto>> GetLanguagesAsync(CancellationToken cancellationToken);
}
