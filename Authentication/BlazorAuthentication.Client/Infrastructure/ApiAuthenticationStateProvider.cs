﻿using BlazorAuthentication.Shared.Account;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorAuthentication.Client.Infrastructure
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrWhiteSpace(savedToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);
            }

            var userInfo = await _httpClient.GetJsonAsync<UserModel>("https://localhost:44380/api/accounts/user");
            var identity = userInfo.IsAuthenticated
                ? new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userInfo.Username) }, "apiauth")
                : new ClaimsIdentity();

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public void MarkUserAsAuthenticated(string username)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }, "apiauth"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
