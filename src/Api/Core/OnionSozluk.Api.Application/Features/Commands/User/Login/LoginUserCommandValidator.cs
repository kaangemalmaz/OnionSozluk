using FluentValidation;
using OnionSozluk.Common.ViewModels.RequestModels;

namespace OnionSozluk.Api.Application.Features.Commands.User
{
    public partial class LoginUserCommandHandler
    {
        // Bu metod geldiği anda daha controllerda yollamadan işlem görür.
        public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
        {
            public LoginUserCommandValidator()
            {
                RuleFor(i => i.EmailAddress)
                    .NotNull()
                    .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                    .WithMessage("{PropertyName} not a valid email address");

                RuleFor(i => i.Password)
                    .NotNull()
                    .MinimumLength(6).WithMessage("{PropertyName} should at least be {MinLenght} characters");
            }
        }
    }
}
