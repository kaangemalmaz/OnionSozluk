using OnionSozluk.Common.Events.User;
using OnionSozluk.Common.Infrastructure.Exceptions;
using OnionSozluk.Common.Infrastructure.Results;
using OnionSozluk.Common.ViewModels.Queries;
using OnionSozluk.WebApp.Infrastructure.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace OnionSozluk.WebApp.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient client;

        public UserService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<UserDetailViewModel> GetUserDetail(Guid? id)
        {
            var userDetail = await client.GetFromJsonAsync<UserDetailViewModel>($"/api/User/{id}");
            return userDetail;
        }

        public async Task<UserDetailViewModel> GetUserDetail(string userName)
        {
            var userDetail = await client.GetFromJsonAsync<UserDetailViewModel>($"/api/User/username/{userName}");
            return userDetail;
        }

        public async Task<bool> UpdateUser(UserDetailViewModel user)
        {
            var res = await client.PostAsJsonAsync($"/api/User/update", user);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> ChangeUserPassword(string oldPassword, string newPassword)
        {
            var command = new ChangeUserPasswordCommand(null, oldPassword, newPassword);
            var httpResponse = await client.PostAsJsonAsync($"/api/User/ChangePassword", command);

            if (httpResponse != null && !httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var responseStr = await httpResponse.Content.ReadAsStringAsync(); //hata içeriğini oku.
                    var validation = JsonSerializer.Deserialize<ValidationResponseModel>(responseStr); // gelen jsoni deserialize
                    responseStr = validation.FlattenErrors; // düz stringe çevir.
                    throw new DatabaseValidationException(responseStr); //öne hata dön.
                }

                return false;
            }

            return httpResponse.IsSuccessStatusCode;
        }
    }
}
