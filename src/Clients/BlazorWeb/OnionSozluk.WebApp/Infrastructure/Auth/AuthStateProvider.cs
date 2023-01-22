using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using OnionSozluk.WebApp.Infrastructure.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnionSozluk.WebApp.Infrastructure.Auth
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        // kullanıcının authenticate mi diye bakacak!

        private readonly ILocalStorageService _localStorageService; // burada token bilgisi o yüzden çağırılır.
        private readonly AuthenticationState anonymous;

        public AuthStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
            anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())); //ben bunu bilmiyorum demektir. identitiesi yok.
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // kullanıcı login oldumu olmadımı bilgisini verecek.
            var apiToken = await _localStorageService.GetToken();

            if (string.IsNullOrEmpty(apiToken)) 
                return anonymous;


            //jwt bilgilerinden claimsler bulunur.
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(apiToken);

            var cp = new ClaimsPrincipal(new ClaimsIdentity(securityToken.Claims, "jwtAuthType"));
            return new AuthenticationState(cp);

        }


        public void NotifyUserLogin(string userName, Guid userId)
        {
            var cp = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "jwtAuthType"));

            var authState = Task.FromResult(new AuthenticationState(cp));

            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(anonymous);
            NotifyAuthenticationStateChanged(authState); 
        }
    }
}
