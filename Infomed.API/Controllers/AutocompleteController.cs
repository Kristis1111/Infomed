using Infomed.Domain.Dto;
using Infomed.Services.Autocomplete;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Infomed.API.Controllers;

public class AutocompleteController(IAutocompleteService autocompleteService) : BaseController
{
    [HttpGet("areasandcitiessearch")]
    public async Task<ActionResult<IEnumerable<AutocompleteOptionDto>>> SearchAreasAndCities([FromQuery, Required, MinLength(2)] string searchTerm, CancellationToken cancellationToken)
    {
        var result = await autocompleteService.SearchAreasAndCitiesAsync(searchTerm, cancellationToken);
        return Ok(result);
    }

    [HttpGet("expertssearch")]
    public async Task<ActionResult<IEnumerable<AutocompleteOptionDto>>> SearchExperts([FromQuery, Required, MinLength(2)] string searchTerm, CancellationToken cancellationToken)
    {
        var result = await autocompleteService.SearchExpertsAsync(searchTerm, cancellationToken);
        return Ok(result);
    }
}
