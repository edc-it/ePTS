using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefLocationType")]
    public class RefLocationType
    {
        public RefLocationType()
        {
            Locations = new HashSet<RefLocation>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Location Type")]
        [Column(Order = 1)]
        public int RefLocationTypeId { get; set; }

        [Display(Name = "Location Type Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the location type")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Location Type")]
        [MaxLength(150)]
        [Column(Order = 3)]
        public string? LocationType { get; set; } = null!;

        [Display(Name = "Location Level")]
        [Column(Order = 4)]
        public int LocationLevel { get; set; }

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 5)]
        public int? SortOrder { get; set; }

        public virtual ICollection<RefLocation> Locations { get; set; }

    }
}

