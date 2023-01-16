using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace OnionSozluk.Api.Application.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddApplicationRegistration(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly(); // çalışan proje altında kendine bağlı olan class libraryleri barındırıyor olur.

            // dependency enjection kütüphaneleri aşağıdaki gibi direk olarak assemblydekileri alsın diye kuruluyor.
            services.AddMediatR(assembly); // bu sayede irequest ve irequesthandlerları bulur.
            services.AddAutoMapper(assembly); 
            services.AddValidatorsFromAssembly(assembly);

            return services;
        } 
    }
}
