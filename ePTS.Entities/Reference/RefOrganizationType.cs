using ePTS.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefOrganizationType")]
    public class RefOrganizationType
    {
        public RefOrganizationType()
        {
            Organizations = new HashSet<Organization>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Organization Type")]
        [Comment("The foreign key identifier of the type of organization or entity. ")]
        [Column(Order = 1)]
        public int RefOrganizationTypeId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Organization Type", Prompt = "Enter the organization type")]
        [MaxLength(150)]
        [Column(Order = 2)]
        public string? OrganizationType { get; set; } = null!;

        [Display(Name = "Organization Type Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the organization type")]
        [Column(Order = 3)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "IsOrganizationUnit", Prompt = "Is it an organization unit? (Yes/No)")]
        [Comment("A Boolean value indicating whether this entity is a container")]
        [Column(Order = 4)]
        public bool IsOrganizationUnit { get; set; }

        [Display(Name = "IsSchool", Prompt = "Is it a school? (Yes/No)")]
        public bool? IsSchool { get; set; }

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 5)]
        public int? SortOrder { get; set; }

        public virtual ICollection<Organization> Organizations { get; set; }

    }
}

