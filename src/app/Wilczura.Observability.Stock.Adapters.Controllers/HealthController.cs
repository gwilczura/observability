using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wilczura.Observability.Common;

namespace Wilczura.Observability.Stock.Adapters.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public ActionResult Get()
    {
        return Ok(SystemInfo.GetInfo());
    }
}
