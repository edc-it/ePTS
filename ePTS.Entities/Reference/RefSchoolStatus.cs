using ePTS.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefSchoolStatus")]
    public class RefSchoolStatus
    {
        public RefSchoolStatus()
        {
            Schools = new HashSet<School>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "School Status")]
        [Column(Order = 1)]
        public int RefSchoolStatusId { get; set; }

        [Display(Name = "School Status Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the school status")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "School Status", Prompt = "Enter the school status")]
        [MaxLength(150)]
        [Column(Order = 3)]
        public string? SchoolStatus { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 4)]
        public int? SortOrder { get; set; }

        public virtual ICollection<School> Schools { get; set; }

    }
}

