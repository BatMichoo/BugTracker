using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class BaseController : ControllerBase
    {
    }
}
