using MediatR;
using OnionSozluk.Api.Application.Interfaces.Repositories;
using OnionSozluk.Common.Events.User;
using OnionSozluk.Common.Infrastructure;
using OnionSozluk.Common.Infrastructure.Exceptions;

namespace OnionSozluk.Api.Application.Features.Commands.User.ChangePassword
{
    public class ChangeUserPasswordCommandHandler : IRequestHandler<ChangeUserPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public ChangeUserPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!request.UserId.HasValue) throw new ArgumentNullException("UserId not be empty");

            var user = await _userRepository.GetSingleAsync(p => p.Id == request.UserId);
            if (user == null) throw new DatabaseValidationException("user not found");

            if (user.Password != PasswordEncryptor.Encrpt(request.OldPassword)) throw new DatabaseValidationException("Password is wrong");

            user.Password = PasswordEncryptor.Encrpt(request.NewPassword);

            var rows = await _userRepository.UpdateAsync(user);

            if (rows > 0) return true;
            return false;

        }
    }
}
