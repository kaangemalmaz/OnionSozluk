using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionSozluk.Api.Application.Interfaces.Repositories;
using OnionSozluk.Common.Infrastructure.Extensions;
using OnionSozluk.Common.ViewModels.Page;
using OnionSozluk.Common.ViewModels.Queries;

namespace OnionSozluk.Api.Application.Features.Queries.GetEntryComments
{
    public class GetEntryCommentsQueryHandler : IRequestHandler<GetEntryCommentsQuery, PagedViewModel<GetEntryCommentsViewModel>>
    {
        private readonly IEntryCommentRepository _entryCommentRepository;

        public GetEntryCommentsQueryHandler(IEntryCommentRepository entryCommentRepository)
        {
            _entryCommentRepository = entryCommentRepository;
        }

        public Task<PagedViewModel<GetEntryCommentsViewModel>> Handle(GetEntryCommentsQuery request, CancellationToken cancellationToken)
        {
            var query = _entryCommentRepository.AsQueryable();

            query.Include(p => p.EntryCommentFavorites)
                 .Include(p => p.EntryCommentVotes)
                 .Include(p => p.CreatedBy)
                 .Where(p => p.EntryId == request.EntryId);

            var list = query.Select(i => new GetEntryCommentsViewModel
            {
                Content = i.Content,
                CreatedByUserName = i.CreatedBy.UserName,
                CreatedDate = i.CreateDate,
                FavoritedCount = i.EntryCommentFavorites.Count,
                Id = i.Id,
                IsFavorited = request.UserId.HasValue && i.EntryCommentFavorites.Any(j => j.CreatedById == request.UserId),
                VoteType = request.UserId.HasValue && i.EntryCommentVotes.Any(j => j.CreateById == request.UserId)
                            ? i.EntryCommentVotes.FirstOrDefault(p => p.CreateById == request.UserId).VoteType
                            : Common.ViewModels.VoteType.None
            });


            var entities = list.GetPaged<GetEntryCommentsViewModel>(request.Page, request.PageSize);
            return entities;

        }
    }
}
