using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionSozluk.Common.ViewModels.RequestModels;

namespace OnionSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
