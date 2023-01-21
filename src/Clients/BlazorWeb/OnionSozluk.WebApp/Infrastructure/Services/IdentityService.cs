using Blazored.LocalStorage;
using OnionSozluk.Common.Infrastructure.Exceptions;
using OnionSozluk.Common.Infrastructure.Results;
using OnionSozluk.Common.ViewModels.Queries;
using OnionSozluk.Common.ViewModels.RequestModels;
using OnionSozluk.WebApp.Infrastructure.Extensions;
using OnionSozluk.WebApp.Infrastructure.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace OnionSozluk.WebApp.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient httpClient;
        private readonly ISyncLocalStorageService syncLocalStorageService;

        public IdentityService(HttpClient httpClient, ISyncLocalStorageService syncLocalStorageService)
        {
            this.httpClient = httpClient;
            this.syncLocalStorageService = syncLocalStorageService;
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(GetUserToken());

        public string GetUserToken()
        {
            return syncLocalStorageService.GetToken();
        }

        public string GetUserName()
        {
            return syncLocalStorageService.GetToken();
        }

        public Guid GetUserId()
        {
            return syncLocalStorageService.GetUserId();
        }

        public async Task<bool> Login(LoginUserCommand command)
        {
            string responseStr;
            var httpResponse = await httpClient.PostAsJsonAsync("/api/User/Login", command);

            if (httpResponse != null && !httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    responseStr = await httpResponse.Content.ReadAsStringAsync();
                    var validation = JsonSerializer.Deserialize<ValidationResponseModel>(responseStr);
                    responseStr = validation.FlattenErrors;
                    throw new DatabaseValidationException(responseStr);
                }

                return false;
            }

            responseStr = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<LoginUserViewModel>(responseStr);

            if (!string.IsNullOrEmpty(response.Token))
            {
                syncLocalStorageService.SetToken(response.Token);
                syncLocalStorageService.SetUserName(response.UserName);
                syncLocalStorageService.SetUserId(response.Id);

                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", response.UserName);

                return true;
            }

            return false;
        }

        public void Logout()
        {
            syncLocalStorageService.RemoveItem(LocalStorageExtension.TokenName);
            syncLocalStorageService.RemoveItem(LocalStorageExtension.UserName);
            syncLocalStorageService.RemoveItem(LocalStorageExtension.UserId);

            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}