using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using OnionSozluk.Api.Application.Extensions;
using OnionSozluk.Api.WebApi.Infrastructure.ActionFilters;
using OnionSozluk.Api.WebApi.Infrastructure.Extensions;
using OnionSozluk.Infrastructure.Persistence.Extensions;
using OnionSozluk.Infrastructure.Persistence.Context;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers(opt => opt.Filters.Add<ValidateModelStateFilter>())
    .AddFluentValidation()
    .ConfigureApiBehaviorOptions(opt =>
    {
        opt.SuppressModelStateInvalidFilter = true; // bizim verdi�imiz filteri �al��t�r demektir.
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.AddInfrastructureRegistration(builder.Configuration);
builder.Services.AddApplicationRegistration();


//Addcors
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder
    .AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod();
}));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;

    var context = sp.GetRequiredService<OnionSozlukContext>();

    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();

        //var seedData = new SeedData();
        //seedData.SeedAsync(builder.Configuration).GetAwaiter().GetResult();

        SeedData.SeedAsync(builder.Configuration).GetAwaiter().GetResult();
    }

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.ConfigureExceptionHandling(app.Environment.IsDevelopment()); //e�er development ise detaylar� g�ster.

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("MyPolicy");

app.MapControllers();

app.Run();
