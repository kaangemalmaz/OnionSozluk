using AutoMapper;
using MediatR;
using OnionSozluk.Api.Application.Interfaces.Repositories;
using OnionSozluk.Common;
using OnionSozluk.Common.Events.User;
using OnionSozluk.Common.Infrastructure;
using OnionSozluk.Common.Infrastructure.Exceptions;
using OnionSozluk.Common.ViewModels.RequestModels;

namespace OnionSozluk.Api.Application.Features.Commands.User.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await _userRepository.GetByIdAsync(request.Id);
            if (dbUser is null) throw new DatabaseValidationException("User not found!");

            var dbEmailAddress = dbUser.EmailAddress;
            //email kontrol ediyoruz. Büyük küçük harf duyarlılığı yok.
            var emailChanged = string.CompareOrdinal(dbEmailAddress, request.EmailAddress) != 0;

            _mapper.Map(request, dbUser);

            var rows = await _userRepository.UpdateAsync(dbUser);

            // Check if email changed => rabbitmq for email confirmation
            if (emailChanged && rows > 0)
            {
                var @event = new UserEmailChangedEvent()
                {
                    OldEmailAddress = dbEmailAddress,
                    NewEmailAddress = dbUser.EmailAddress
                };

                QueueFactory.SendMessageToExchange
                    (
                    exchangeName: SozlukConstants.UserExchangeName,
                    exchangeType: SozlukConstants.DefaultExchangeType,
                    queueName: SozlukConstants.UserEmailChangedQueueName,
                    obj: @event
                    );

                // email adresi değişti ise yeniden email onaylaması gerekir o yüzden yeniden set edilir.
                dbUser.EmailConfirmed = false;
                await _userRepository.UpdateAsync(dbUser);
            }

            return dbUser.Id;
        }
    }
}
