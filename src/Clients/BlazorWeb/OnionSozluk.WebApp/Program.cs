using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnionSozluk.WebApp;
using OnionSozluk.WebApp.Infrastructure.Services;
using OnionSozluk.WebApp.Infrastructure.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("WebApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001");
});

builder.Services.AddScoped(sp =>
{
    var clientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return clientFactory.CreateClient("WebApiClient");
});

//�sttekiler ile ayn� i�i yapar ama yap�n�n nas�l i�ledi�inin ayr�t�s� yukar�dad�r.
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


builder.Services.AddTransient<IVoteService, VoteService>();
builder.Services.AddTransient<IEntryService, EntryService>();
builder.Services.AddTransient<IFavService, FavService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IIdentityService, IdentityService>();


builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
