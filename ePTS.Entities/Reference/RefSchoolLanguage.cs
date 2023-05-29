using ePTS.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefSchoolLanguage")]
    public class RefSchoolLanguage
    {
        public RefSchoolLanguage()
        {
            Schools = new HashSet<School>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "School Language")]
        [Column(Order = 1)]
        public int RefSchoolLanguageId { get; set; }

        [Display(Name = "School Language Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the school language")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "School Language", Prompt = "Enter the school language")]
        [MaxLength(150)]
        [Column(Order = 3)]
        public string? SchoolLanguage { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 4)]
        public int SortOrder { get; set; }

        public virtual ICollection<School> Schools { get; set; }

    }
}

