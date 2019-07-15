using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorAuthentication.Shared.Account
{
    public class RegisterResult
    {
        public bool Successful { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
