using Microsoft.AspNetCore.Mvc;

namespace Infomed.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class BaseController : ControllerBase
{
}
