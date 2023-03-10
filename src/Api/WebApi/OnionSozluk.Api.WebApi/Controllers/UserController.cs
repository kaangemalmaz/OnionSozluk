using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionSozluk.Api.Application.Features.Commands.User.ConfirmEmail;
using OnionSozluk.Api.Application.Features.Queries.GetUserDetail;
using OnionSozluk.Common.Events.User;
using OnionSozluk.Common.ViewModels.RequestModels;

namespace OnionSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetailById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetUserDetailQuery { UserId = id });
            return Ok(result);
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserDetailByUsername([FromRoute] string username)
        {
            var result = await _mediator.Send(new GetUserDetailQuery { UserId = Guid.Empty, UserName = username });
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("Register")]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("Update")]
        [Authorize]

        public async Task<IActionResult> Update([FromBody] UpdateUserCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPasswordCommand command)
        {
            if (!command.UserId.HasValue)
                command.UserId = UserId;

            var guid = await _mediator.Send(command);

            return Ok(guid);
        }

        [HttpPost]
        [Route("Confirm")]
        public async Task<IActionResult> ConfirmEMail(Guid id)
        {
            var guid = await _mediator.Send(new ConfirmEmailCommand() { ConfirmationId = id });

            return Ok(guid);
        }

    }
}
