using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionSozluk.Api.Application.Features.Queries.GetEntries;
using OnionSozluk.Api.Application.Features.Queries.GetEntryComments;
using OnionSozluk.Api.Application.Features.Queries.GetEntryDetail;
using OnionSozluk.Api.Application.Features.Queries.GetMainPageEntries;
using OnionSozluk.Api.Application.Features.Queries.GetUserEntries;
using OnionSozluk.Common.ViewModels.Queries;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetEntryDetailQuery(id, UserId));
            return Ok(result);
        }

        [HttpGet("Comments/{id}")]
        public async Task<IActionResult> GetEntryComments(Guid id, int page, int pageSize)
        {
            var result = await _mediator.Send(new GetEntryCommentsQuery(id, UserId, page, pageSize));
            return Ok(result);
        }

        [HttpGet("UserEntries")]
        public async Task<IActionResult> GetUserEntries(string userName, Guid userId, int page, int pageSize)
        {
            //if (userId == Guid.Empty && string.IsNullOrEmpty(userName))
            //    userId = UserId.Value;

            var result = await _mediator.Send(new GetUserEntriesQuery(userId, userName, page, pageSize));
            return Ok(result);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] SearchEntryQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
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
