using ePTS.Entities.Identity;
using ePTS.Entities.Reference;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Core
{
    // The Organization class represents an organization entity and inherits from the BaseEntity class.
    [Table("Organization")]
    public class Organization : BaseEntity
    {
        public Organization()
        {
            // Initialize the collections in the constructor.
            ApplicationUserOrganizations = new HashSet<ApplicationUserOrganization>();
            Organizations = new HashSet<Organization>();
        }

        // Represents the unique identifier for each organization.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Organization")]
        [Comment("Unique identifier for each organization, such as educational institutions or school districts")]
        [Column(Order = 1)]
        public Guid OrganizationId { get; set; }

        // Represents the registration date of the organization.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the organization was registered or added to the database")]
        [DataType(DataType.Date)]
        [Column(Order = 2)]
        public DateTime RegistrationDate { get; set; }

        // Represents the code of the organization.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Organization Code", Prompt = "Enter the organization code")]
        [MaxLength(100)]
        [Comment("A short code that represents the unique organization")]
        [Column(Order = 3)]
        public string? Code { get; set; } = null!;

        // Represents the name of the organization.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Organization Name", Prompt = "Enter the organization name")]
        [MaxLength(255)]
        [Comment("The name of the organization")]
        [Column(Order = 4)]
        public string? OrganizationName { get; set; } = null!;

        // Represents the type of the organization.
        [Display(Name = "Organization Type", Prompt = "Select the organization type")]
        [Comment("A reference to the type of organization to which this entity belongs (e.g. school, district, etc.)")]
        [Column(Order = 5)]
        public int? RefOrganizationTypeId { get; set; }

        // Represents the location of the organization.
        [Display(Name = "Location", Description = "Geographic Location", Prompt = "Select the location")]
        [MaxLength(25)]
        [Comment("A reference to the geographic location of the organization")]
        [Column(Order = 6)]
        public string? RefLocationId { get; set; }

        // Represents the address of the organization.
        [Display(Name = "Address", Description = "Address or street name", Prompt = "Enter the address")]
        [MaxLength(384)]
        [Comment("The physical address of the organization, which could include street name, street number, zip code, etc")]
        [Column(Order = 7)]
        public string? Address { get; set; }

        // Represents the parent organization of the organization.
        [Display(Name = "Parent Organization", Prompt = "Select the parent organization")]
        [Comment("Reference to the parent organization of this organization, if any (e.g. a district could have multiple schools as its child organizations)")]
        [Column(Order = 8)]
        public Guid? ParentOrganizationId { get; set; }

        // Navigation property referencing the ParentOrganization entity.
        [ForeignKey("ParentOrganizationId")]
        [Display(Name = "Parent Organization")]
        public virtual Organization? ParentOrganizations { get; set; }

        // Navigation property referencing the RefOrganizationType entity.
        [ForeignKey("RefOrganizationTypeId")]
        [Display(Name = "Organization Type")]
        public virtual RefOrganizationType? OrganizationTypes { get; set; } = null!;

        // Navigation property referencing the RefLocation entity.
        [ForeignKey("RefLocationId")]
        [Display(Name = "Location")]
        public virtual RefLocation? Locations { get; set; }

        // Represents the latitude coordinates of the organization's location.
        [Display(Name = "Latitude", Prompt = "Enter the latitude")]
        [Comment("Latitude coordinates of the organization's location in decimal degrees format")]
        [Column(Order = 9)]
        public double? Latitude { get; set; }

        // Represents the longitude coordinates of the organization's location.
        [Display(Name = "Longitude", Prompt = "Enter the longitude")]
        [Comment("Longitude coordinates of the organization's location in decimal degrees format")]
        [Column(Order = 10)]
        public double? Longitude { get; set; }

        // Represents whether the entity is an organization unit.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "IsOrganizationUnit", Prompt = "Is it an organization unit? (Yes/No)")]
        [Comment("A Boolean value indicating whether this entity is a container")]
        [Column(Order = 11)]
        public bool IsOrganizationUnit { get; set; }

        // Collection navigation property representing the application users associated with the organization.
        public virtual ICollection<ApplicationUserOrganization> ApplicationUserOrganizations { get; set; }

        // Collection navigation property representing the child organizations of the organization.
        public virtual ICollection<Organization> Organizations { get; set; }

        
        
    }
}


