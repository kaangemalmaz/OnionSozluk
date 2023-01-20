using MediatR;
using OnionSozluk.Common.ViewModels.Page;
using OnionSozluk.Common.ViewModels.Queries;

namespace OnionSozluk.Api.Application.Features.Queries.GetMainPageEntries
{
    public class GetMainPageEntriesQuery : BasePagedQuery, IRequest<PagedViewModel<GetEntryDetailViewModel>>
    {
        public GetMainPageEntriesQuery(Guid? userId, int page, int pageSize) : base(page, pageSize)
        {
            UserId = userId;
        }

        public Guid? UserId { get; set; }

    }
}
