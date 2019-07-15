using BlazorAuthentication.Data.Models;
using BlazorAuthentication.Shared.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAuthentication.Server.Controllers
{
    public class AccountsController : ApiController
    {
        //private static UserModel LoggedOutUser = new UserModel { IsAuthenticated = false };

        private readonly UserManager<ApplicationUser> userManager;

        public AccountsController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterInputModel model)
        {
            var newUser = new IdentityUser { UserName = model.Email, Email = model.Email };

            var result = await userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return BadRequest(new RegisterResult { Successful = false, Errors = errors });

            }

            return Ok(new RegisterResult { Successful = true });
        }

        [HttpGet("user")]
        public IActionResult GetUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userModel = new UserModel
                {
                    Email = User.Identity.Name,
                    IsAuthenticated = true
                };

                return Ok(userModel);
            }

            return Ok(LoggedOutUser);
        }
    }
}
