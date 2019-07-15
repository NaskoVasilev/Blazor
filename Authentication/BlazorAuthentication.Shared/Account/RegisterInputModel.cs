using System.ComponentModel.DataAnnotations;

namespace BlazorAuthentication.Shared.Account
{
    public class RegisterInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }
    }
}
