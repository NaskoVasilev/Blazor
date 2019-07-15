using BlazorAuthentication.Data.Models;
using BlazorAuthentication.Shared.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAuthentication.Server.Controllers
{
    public class AccountsController : ApiController
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountsController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<RegisterResult> Post([FromBody]RegisterInputModel model)
        {
            var newUser = new ApplicationUser { UserName = model.Username, Email = model.Email, FullName = model.FullName };

            var result = await userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return new RegisterResult { Successful = false, Errors = errors };

            }

            return new RegisterResult { Successful = true };
        }

        [HttpGet("user")]
        public IActionResult GetUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userModel = new UserModel
                {
                    Username = User.Identity.Name,
                    IsAuthenticated = true
                };

                return Ok(userModel);
            }

            return Ok(new UserModel { IsAuthenticated = false });
        }
    }
}
