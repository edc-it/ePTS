using ePTS.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Identity
{
    // Represents the relationship between ApplicationUser and Organization entities.
    [Table("ApplicationUserOrganization")]
    [PrimaryKey(nameof(UserId), nameof(OrganizationId))]
    [Comment("Represents the relationship between ApplicationUser and Organization entities.")]
    public class ApplicationUserOrganization : BaseEntity
    {
        // Unique identifier for the user.
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "User")]
        [Column(Order = 1)]
        public Guid UserId { get; set; }

        // Reference to the organization.
        [Display(Name = "Organization", Prompt = "Select the organization name")]
        [Column(Order = 2)]
        public Guid? OrganizationId { get; set; }

        // Navigation property referencing the ApplicationUser entity.
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? Users { get; set; }

        // Navigation property referencing the Organization entity.
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization? Organizations { get; set; }
    }

}

