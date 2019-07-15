using BlazorAuthentication.Shared.Account;
using BlazorAuthentication.Shared.Login;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlazorAuthentication.Client.Infrastructure
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<RegisterResult> Register(RegisterInputModel registerModel)
        {
            var result = await _httpClient.PostJsonAsync<RegisterResult>("https://localhost:44380/api/accounts", registerModel);

            return result;
        }

        public async Task<LoginResult> Login(LoginInputModel loginModel)
        {
            var result = await _httpClient.PostJsonAsync<LoginResult>("https://localhost:44380/api/Login", loginModel);

            if (result.Successful)
            {
                await _localStorage.SetItemAsync("authToken", result.Token);
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Username);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

                return result;
            }

            return result;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
