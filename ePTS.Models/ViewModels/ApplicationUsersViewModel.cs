using System.ComponentModel.DataAnnotations;

namespace ePTS.Models.ViewModels
{
    public class ApplicationUsersViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "UserName")]
        public string? UserName { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }
        
        [Display(Name = "Role")]
        public string? Role { get; set; }

        [Display(Name = "Access Failed Count")]
        public int? AccessFailedCount { get; set; }

        [Display(Name = "Lockout Enabled")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Lockout End")]
        public DateTimeOffset? LockoutEnd { get; set; }

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
        public string? RoleId { get; set; }
        public string? Organization { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? ParentOrganization1 { get; set; }
        public string? ParentOrganization2 { get; set; }
        public string? ParentOrganization3 { get; set; }
        public string? MoGE { get; set; }
        public string? District { get; set; }
        public string? Province { get; set; }
        public string? Zone { get; set; }
        public int? OrganizationTypeId { get; set; }
        public string? OrganizationType { get; set; }
    }
}
