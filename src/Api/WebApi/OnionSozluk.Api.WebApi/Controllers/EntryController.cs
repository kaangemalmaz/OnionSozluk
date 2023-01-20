using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionSozluk.Api.Application.Features.Queries.GetEntries;
using OnionSozluk.Api.Application.Features.Queries.GetMainPageEntries;
using OnionSozluk.Common.ViewModels.Page;
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
        public async Task<IActionResult> GetEntries([FromQuery] GetEntriesQuery query)
        {

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetMainPageEntries")]
        public async Task<IActionResult> GetMainPageEntries(int page, int pageSize)
        {

            var result = await _mediator.Send(new GetMainPageEntriesQuery(UserId, page, pageSize));
            return Ok(result);
        }
    }
}
