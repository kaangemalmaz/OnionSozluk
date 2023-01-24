using OnionSozluk.Projections.UserService;
using OnionSozluk.Projections.UserService.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddTransient<UserService>();
        services.AddTransient<EmailService>();
    })
    .Build();

await host.RunAsync();
