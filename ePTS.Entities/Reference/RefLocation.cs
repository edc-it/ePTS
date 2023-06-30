using ePTS.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefLocation")]
    public class RefLocation
    {
        public RefLocation()
        {
            Organizations = new HashSet<Organization>();
            Locations = new HashSet<RefLocation>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Location")]
        [MaxLength(25)]
        [Comment("A unique identifier for the geographic location record of the Organization")]
        [Column(Order = 1)]
        public string? RefLocationId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Location Name")]
        [MaxLength(255)]
        [Comment("The name of the location")]
        [Column(Order = 2)]
        public string? LocationName { get; set; } = null!;

        [Display(Name = "Location Type")]
        [Comment("A reference to the type of location (e.g., country, state, province, city, etc.)")]
        [Column(Order = 3)]
        public int? RefLocationTypeId { get; set; }

        [Display(Name = "Parent Location")]
        [MaxLength(25)]
        [Comment("A reference to the parent location of this location.")]
        [Column(Order = 4)]
        public string? ParentLocationId { get; set; }

        [Display(Name = "Latitude", Prompt = "Enter the latitude")]
        [Comment("Latitude coordinates of the location in decimal degrees format")]
        [Column(Order = 5)]
        public double? Latitude { get; set; }

        [Display(Name = "Longitude", Prompt = "Enter the longitude")]
        [Comment("Longitude coordinates of the location in decimal degrees format")]
        [Column(Order = 6)]
        public double? Longitude { get; set; }

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 7)]
        public int? SortOrder { get; set; }

        [ForeignKey("RefLocationTypeId")]
        [Display(Name = "Location Type")]
        public virtual RefLocationType? LocationTypes { get; set; }

        [ForeignKey("ParentLocationId")]
        [Display(Name = "Parent Location")]
        public virtual RefLocation? ParentLocations { get; set; }

        public virtual ICollection<RefLocation> Locations { get; set; }
        public virtual ICollection<Organization> Organizations { get; set; }

    }
}

