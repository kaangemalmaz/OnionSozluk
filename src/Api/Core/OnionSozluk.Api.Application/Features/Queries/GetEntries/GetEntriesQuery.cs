using MediatR;
using OnionSozluk.Common.ViewModels.Queries;

namespace OnionSozluk.Api.Application.Features.Queries.GetEntries
{
    public class GetEntriesQuery : IRequest<List<GetEntriesViewModel>>
    {
        public bool TodaysEntries { get; set; }

        public int Count { get; set; } = 100;
    }
}
