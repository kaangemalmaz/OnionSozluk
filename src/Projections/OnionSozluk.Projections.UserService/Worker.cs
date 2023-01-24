using OnionSozluk.Common;
using OnionSozluk.Common.Events.User;
using OnionSozluk.Common.Infrastructure;
using OnionSozluk.Projections.UserService.Services;

namespace OnionSozluk.Projections.UserService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly Services.UserService userService;
        private readonly EmailService emailService;

        public Worker(ILogger<Worker> logger, Services.UserService userService, EmailService emailService)
        {
            _logger = logger;
            this.userService = userService;
            this.emailService = emailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            QueueFactory.CreateBasicConsumer()
                .EnsureExchange(SozlukConstants.UserExchangeName, SozlukConstants.DefaultExchangeType)
                .EnsureQueue(SozlukConstants.UserEmailChangedQueueName, SozlukConstants.UserExchangeName)
                .Receive<UserEmailChangedEvent>(user =>
                {
                    // db insert
                    var confirmationId = userService.CreateEmailConfirmation(user).GetAwaiter().GetResult();
                    //email link
                    var link = emailService.GenerateConfirmationLink(confirmationId);
                    //send email
                    emailService.SendEmail(user.NewEmailAddress, link).GetAwaiter().GetResult(); ;

                }).StartConsuming(SozlukConstants.UserEmailChangedQueueName);
        }
    }
}