using ePTS.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefSchoolLocation")]
    public class RefSchoolLocation
    {
        public RefSchoolLocation()
        {
            Schools = new HashSet<School>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "School Location")]
        [Comment("The foreign key identifier of the type of location of the school (i.e. Urban, Rural).")]
        [Column(Order = 1)]
        public int RefSchoolLocationId { get; set; }

        [Display(Name = "School Location Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the school location")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "School Location", Prompt = "Enter the school location")]
        [MaxLength(150)]
        [Column(Order = 3)]
        public string? SchoolLocation { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 4)]
        public int? SortOrder { get; set; }

        public virtual ICollection<School> Schools { get; set; }

    }
}

