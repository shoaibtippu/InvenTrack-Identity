using Microsoft.AspNetCore.Mvc;

namespace Hexagonal.Adapters.Rest.Controllers
{
    /// <summary>
    /// Base controller for the application.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    public class BaseController : ControllerBase
    {
    }
}
