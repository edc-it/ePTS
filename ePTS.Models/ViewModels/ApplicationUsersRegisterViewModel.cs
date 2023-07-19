using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ePTS.Models.ViewModels
{
    public class ApplicationUsersRegisterViewModel
    {
        [Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "White spaces are not allowed")]
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Display(Name = "Role")]
        public Guid RoleId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public Guid? OrganizationId { get; set; }

        public List<string>? Organizations { get; set; }
    }
}
