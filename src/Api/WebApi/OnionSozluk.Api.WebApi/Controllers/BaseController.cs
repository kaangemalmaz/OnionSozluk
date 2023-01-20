using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OnionSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public Guid? UserId => new Guid(); //new(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
}
