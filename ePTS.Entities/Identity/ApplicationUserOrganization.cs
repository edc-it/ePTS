using ePTS.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Identity
{
    [Table("ApplicationUserOrganization")]
    [PrimaryKey(nameof(UserId), nameof(OrganizationId))]
    public class ApplicationUserOrganization : BaseEntity
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "User")]
        [Column(Order = 1)]
        public Guid UserId { get; set; }

        [Display(Name = "Organization", Prompt = "Select the organization name")]
        [Column(Order = 2)]
        public Guid? OrganizationId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? Users { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization? Organizations { get; set; }



    }
}

