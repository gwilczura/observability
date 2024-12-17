using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wilczura.Observability.Common;

namespace Wilczura.Observability.Products.Adapters.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public ActionResult Get()
    {
        return Ok(SystemInfo.GetInfo());
    }
}
