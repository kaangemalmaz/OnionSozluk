using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionSozluk.Api.Application.Interfaces.Repositories;
using OnionSozluk.Common.ViewModels.Queries;

namespace OnionSozluk.Api.Application.Features.Queries.SearchEntry
{
    public class SearchEntryQueryHandler : IRequestHandler<SearchEntryQuery, List<SearchEntryViewModel>>
    {
        private readonly IEntryRepository _entryRepository;

        public SearchEntryQueryHandler(IEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public async Task<List<SearchEntryViewModel>> Handle(SearchEntryQuery request, CancellationToken cancellationToken)
        {
            var result = _entryRepository.
                Get(i => EF.Functions.Like(i.Subject, $"{request.SearchText}%")).
                Select(i => new SearchEntryViewModel
                {
                    Id = i.Id,
                    Subject = i.Subject
                });


            return await result.ToListAsync();
        }
    }
}
