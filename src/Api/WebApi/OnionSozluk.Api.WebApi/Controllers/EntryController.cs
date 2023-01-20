using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionSozluk.Api.Application.Features.Queries.GetEntries;
using OnionSozluk.Common.ViewModels.RequestModels;

namespace OnionSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : BaseController
    {
        private readonly IMediator _mediator;

        public EntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateEntry")]
        public async Task<IActionResult> CreateEntry([FromBody] CreateEntryCommand command)
        {
            command.CreatedById ??= UserId;

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("CreateEntryComment")]
        public async Task<IActionResult> CreateEntryComment([FromBody] CreateEntryCommentCommand command)
        {
            command.CreatedById ??= UserId;

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpGet("GetEntries")]
        public async Task<IActionResult> GetEntries([FromQuery] GetEntriesQuery command)
        {

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
