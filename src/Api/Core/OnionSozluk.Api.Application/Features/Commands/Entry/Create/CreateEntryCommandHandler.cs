using AutoMapper;
using OnionSozluk.Api.Application.Interfaces.Repositories;
using OnionSozluk.Common.ViewModels.RequestModels;

namespace OnionSozluk.Api.Application.Features.Commands.Entry.Create
{
    public class CreateEntryCommandHandler
    {
        private readonly IEntryRepository entryRepository;
        private readonly IMapper mapper;

        public CreateEntryCommandHandler(IEntryRepository entryRepository, IMapper mapper)
        {
            this.entryRepository = entryRepository;
            this.mapper = mapper;
        }

        public async Task<Guid> Handle(CreateEntryCommand request, CancellationToken cancellationToken)
        {
            var dbEntry = mapper.Map<Domain.Models.Entry>(request);

            await entryRepository.AddAsync(dbEntry);

            return dbEntry.Id;
        }
    }
}
