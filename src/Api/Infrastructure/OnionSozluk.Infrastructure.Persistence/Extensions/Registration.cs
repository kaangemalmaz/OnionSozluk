using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnionSozluk.Api.Application.Interfaces.Repositories;
using OnionSozluk.Infrastructure.Persistence.Context;
using OnionSozluk.Infrastructure.Persistence.Repositories;

namespace OnionSozluk.Infrastructure.Persistence.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OnionSozlukContext>(conf =>
            {
                var connStr = configuration["OnionSozlukDbConnectionDbContext"];
                conf.UseSqlServer(connStr, opt =>
                {
                    opt.EnableRetryOnFailure(); // sql servera bağlanırken bir hata oluşursa yeniden denesin diye bir örnek!
                });
            });

            // burası sadece seed data için 1 kere çalışacak olan kısımdır.
            var seedData = new SeedData();
            seedData.SeedAsync(configuration).GetAwaiter().GetResult(); // burayı bekle sonucunu al demektir. 


            // injection
            services.AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>();
            services.AddScoped<IEntryCommentFavoriteRepository, EntryCommentFavoriteRepository>();
            services.AddScoped<IEntryCommentRepository, EntryCommentRepository>();
            services.AddScoped<IEntryCommentVoteRepository, EntryCommentVoteRepository>();
            services.AddScoped<IEntryFavoriteRepository, EntryFavoriteRepository>();
            services.AddScoped<IEntryRepository, EntryRepository>();
            services.AddScoped<IEntryVoteRepository, EntryVoteRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
