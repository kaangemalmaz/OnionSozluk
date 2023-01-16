using MediatR;
using OnionSozluk.Common.ViewModels.Queries;

namespace OnionSozluk.Common.ViewModels.RequestModels
{
    public class LoginUserCommand : IRequest<LoginUserViewModel>
    {
        public string EmailAddress { get;  set; }
        public string Password { get;  set; }

        public LoginUserCommand()
        {
        }

        public LoginUserCommand(string emailAddress, string password)
        {
            EmailAddress = emailAddress;
            Password = password;
        }
    }
}
