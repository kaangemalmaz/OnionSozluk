using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionSozluk.Api.Application.Interfaces.Repositories;
using OnionSozluk.Common.Infrastructure.Extensions;
using OnionSozluk.Common.ViewModels.Page;
using OnionSozluk.Common.ViewModels.Queries;

namespace OnionSozluk.Api.Application.Features.Queries.GetUserEntries
{
    public class GetUserEntriesQueryHandler : IRequestHandler<GetUserEntriesQuery, PagedViewModel<GetUserEntriesDetailViewModel>>
    {
        private readonly IEntryRepository _entryRepository;

        public GetUserEntriesQueryHandler(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public async Task<PagedViewModel<GetUserEntriesDetailViewModel>> Handle(GetUserEntriesQuery request, CancellationToken cancellationToken)
        {
            var query = _entryRepository.AsQueryable();
            if (request.UserId.HasValue && request.UserId != Guid.Empty)
                query = query.Where(i => i.CreatedById == request.UserId);
            else if (!String.IsNullOrEmpty(request.UserName))
                query = query.Where(i => i.CreatedBy.UserName == request.UserName);
            else
                return null;

            query = query.Include(i => i.EntryFavorites)
                         .Include(i => i.CreatedBy);

            var list = query.Select(i => new GetUserEntriesDetailViewModel
            {
                Content = i.Content,
                CreatedByUserName = i.CreatedBy.UserName,
                CreatedDate = i.CreateDate,
                FavoritedCount = i.EntryFavorites.Count,
                Id = i.Id,
                IsFavorited = request.UserId.HasValue && i.EntryFavorites.Any(j => j.CreatedById == request.UserId),
                Subject = i.Subject
            });


            var entities = await list.GetPaged<GetUserEntriesDetailViewModel>(request.Page, request.PageSize);

            return entities;

        }
    }
}
