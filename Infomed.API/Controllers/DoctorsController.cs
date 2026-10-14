using Infomed.Domain.Dto;
using Infomed.Domain.Queries;
using Infomed.Domain.Responses;
using Infomed.Services.Doctors;
using Microsoft.AspNetCore.Mvc;

namespace Infomed.API.Controllers;

public class DoctorsController(IDoctorService doctorService) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<DoctorDto>>> GetDoctors([FromQuery] DoctorSearchQuery query, CancellationToken cancellationToken)
    {
        var result = await doctorService.GetDoctorsAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:int}/contact")]
    public async Task<IActionResult> SubmitContactRequest(int id, [FromBody] ContactRequestDto request, CancellationToken cancellationToken)
    {
        var saved = await doctorService.SaveContactRequestAsync(id, request, cancellationToken);
        return saved ? StatusCode(StatusCodes.Status201Created) : NotFound();
    }
}
