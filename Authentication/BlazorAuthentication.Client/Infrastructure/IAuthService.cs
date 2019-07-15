using BlazorAuthentication.Shared.Account;
using BlazorAuthentication.Shared.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAuthentication.Client.Infrastructure
{
    public interface IAuthService
    {
        Task<RegisterResult> Register(RegisterInputModel registerModel);

        Task<LoginResult> Login(LoginInputModel loginModel);

        Task Logout();
    }
}
