using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wilczura.Observability.Prices.Adapters.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public ActionResult Get()
    {
        return Ok();
    }
}
