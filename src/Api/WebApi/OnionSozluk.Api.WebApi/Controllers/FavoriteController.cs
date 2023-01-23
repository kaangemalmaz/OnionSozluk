using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnionSozluk.Api.Application.Features.Commands.Entry.CreateFav;
using OnionSozluk.Api.Application.Features.Commands.Entry.DeleteFav;
using OnionSozluk.Api.Application.Features.Commands.EntryComment.CreateFav;
using OnionSozluk.Api.Application.Features.Commands.EntryComment.DeleteFav;

namespace OnionSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : BaseController
    {
        private readonly IMediator _mediator;

        public FavoriteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("entry/{entryId}")]
        public async Task<IActionResult> CreateEntryFav([FromRoute] Guid entryId)
        {
            var result = await _mediator.Send(new CreateEntryFavCommand(entryId, UserId.Value));
            return Ok(result);
        }

        [HttpPost("deleteentryfav/{entryId}")]
        public async Task<IActionResult> DeleteEntryFav([FromRoute] Guid entryId)
        {
            var result = await _mediator.Send(new DeleteEntryFavCommand(entryId, UserId.Value));
            return Ok(result);
        }

        [HttpPost("entrycomment/{entryCommentId}")]
        public async Task<IActionResult> CreateEntryCommentFav([FromRoute] Guid entryCommentId)
        {
            var result = await _mediator.Send(new CreateEntryCommentFavCommand(entryCommentId, UserId.Value));
            return Ok(result);
        }

        [HttpPost("deleteentrycommentfav/{entryCommentId}")]
        public async Task<IActionResult> DeleteEntryCommentFav([FromRoute] Guid entryCommentId)
        {
            var result = await _mediator.Send(new DeleteEntryCommentFavCommand(entryCommentId, UserId.Value));
            return Ok(result);
        }

    }
}
