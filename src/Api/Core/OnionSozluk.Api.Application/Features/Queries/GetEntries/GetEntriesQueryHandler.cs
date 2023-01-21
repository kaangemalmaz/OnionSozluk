using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionSozluk.Api.Application.Interfaces.Repositories;
using OnionSozluk.Api.Domain.Models;
using OnionSozluk.Common.ViewModels.Queries;

namespace OnionSozluk.Api.Application.Features.Queries.GetEntries
{
    public class GetEntriesQueryHandler : IRequestHandler<GetEntriesQuery, List<GetEntriesViewModel>>
    {
        private readonly IEntryRepository _entryRepository;
        private readonly IMapper _mapper;
        public GetEntriesQueryHandler(IEntryRepository entryRepository, IMapper mapper)
        {
            _entryRepository = entryRepository;
            _mapper = mapper;
        }

        public async Task<List<GetEntriesViewModel>> Handle(GetEntriesQuery request, CancellationToken cancellationToken)
        {
            var query = _entryRepository.AsQueryable();

            if (request.TodaysEntries)
                query = query.Where(i => i.CreateDate >= DateTime.Now.Date)
                             .Where(i => i.CreateDate <= DateTime.Now.AddDays(1).Date);


            query = query.Include(i => i.EntryComments)
                         .OrderBy(i => Guid.NewGuid()) //gelişi güzel sırala demektir.
                         .Take(request.Count);


            // burada sadece gelen requestte göre veritabanından sorgulama yapmasını sağlar. bu sayede performans artışı sağlar.
            return await query.ProjectTo<GetEntriesViewModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        }
    }
}
