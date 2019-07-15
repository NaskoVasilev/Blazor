using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorAuthentication.Shared.Account
{
    public class UserModel
    {
        public string Username { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}
