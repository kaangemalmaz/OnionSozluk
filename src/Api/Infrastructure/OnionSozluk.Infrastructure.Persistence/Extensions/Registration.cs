using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnionSozluk.Infrastructure.Persistence.Context;

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

            return services;
        }
    }
}
