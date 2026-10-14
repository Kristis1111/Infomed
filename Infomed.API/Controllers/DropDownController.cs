using Infomed.Domain.Dto;
using Infomed.Services.DropDowns;
using Microsoft.AspNetCore.Mvc;

namespace Infomed.API.Controllers;

public class DropDownController(IDropDownService dropDownService) : BaseController
{
    [HttpGet("languages")]
    public async Task<ActionResult<IReadOnlyList<AutocompleteOptionDto>>> GetLanguages(CancellationToken cancellationToken)
    {
        var languages = await dropDownService.GetLanguagesAsync(cancellationToken);
        return Ok(languages);
    }
}
