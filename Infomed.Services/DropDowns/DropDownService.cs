using Infomed.Data.DropDowns;
using Infomed.Domain.Dto;

namespace Infomed.Services.DropDowns;

// Logger not implemented on purpose

public class DropDownService(IDropDownDataService dropDownDataService) : IDropDownService
{
    public Task<IEnumerable<AutocompleteOptionDto>> GetLanguagesAsync(CancellationToken cancellationToken)
    {
        return dropDownDataService.GetLanguagesAsync(cancellationToken);
    }
}
