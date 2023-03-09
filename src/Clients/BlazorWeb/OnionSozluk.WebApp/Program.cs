using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OnionSozluk.WebApp;
using OnionSozluk.WebApp.Infrastructure.Auth;
using OnionSozluk.WebApp.Infrastructure.Services;
using OnionSozluk.WebApp.Infrastructure.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthTokenHandler>();

builder.Services.AddHttpClient("WebApiClient", client =>
{
    // web assembly bilgisayarda yani localhostda çalışır bu sebeple biz buraya getirip container ismini yazamıyoruz. 
    // Eğer yazarsak bizim web assemblymiz bu containeri çözümleyemeyecektir.
    //client.BaseAddress = new Uri("http://localhost:5001");
    string requestUrl = builder.Configuration["RequestUrl"];
    client.BaseAddress = new Uri(requestUrl);
})
    .AddHttpMessageHandler<AuthTokenHandler>(); // her httpclientda header authorizationa ekleme yapar.

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

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddAuthorizationCore(); // authentication mekanizmas�

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
