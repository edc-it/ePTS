using System.ComponentModel.DataAnnotations;

namespace ePTS.Models.ViewModels
{
    public class ApplicationUsersEditViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Role")]
        public string? RoleId { get; set; }

        [Display(Name = "Lockout Enabled")]
        public bool LockoutEnabled { get; set; } = false;
        public Guid? OrganizationId { get; set; }

        public List<string>? Organizations { get; set; }
        public Guid? OrganizationIdInitialValue { get; set; }

        public List<string>? OrganizationsInitialValue { get; set; }
    }
}
