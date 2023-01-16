using AutoMapper;
using MediatR;
using OnionSozluk.Api.Application.Interfaces.Repositories;
using OnionSozluk.Common;
using OnionSozluk.Common.Events.User;
using OnionSozluk.Common.Infrastructure;
using OnionSozluk.Common.Infrastructure.Exceptions;
using OnionSozluk.Common.ViewModels.RequestModels;

namespace OnionSozluk.Api.Application.Features.Commands.User.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existsUser = await _userRepository.GetSingleAsync(p => p.EmailAddress == request.EmailAddress);
            if (existsUser is not null) throw new DatabaseValidationException("User Already Exists!");

            var dbUser = _mapper.Map<Domain.Models.User>(request);

            var rows = await _userRepository.AddAsync(dbUser); // int büyük 0 döner veritabanı kaydı sonrası.

            // Email Changed / Created => rabbitmq for email confirmation
            if (rows > 0)
            {
                // yeni kayıt olduğu için oldemail'i yok!
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = null,
                    NewEmailAddress = dbUser.EmailAddress
                };
                QueueFactory.SendMessageToExchange
                    (
                    exchangeName: SozlukConstants.UserExchangeName,
                    exchangeType: SozlukConstants.DefaultExchangeType,
                    queueName: SozlukConstants.UserEmailChangedQueueName,
                    obj: @event
                    );
            }

            return dbUser.Id;
        }
    }
}
