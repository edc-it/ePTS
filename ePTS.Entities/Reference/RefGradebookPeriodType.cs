using ePTS.Entities.Gradebooks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefGradebookPeriodType")]
    public class RefGradebookPeriodType
    {
        public RefGradebookPeriodType()
        {
            GradebookPeriods = new HashSet<GradebookPeriod>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Period Type")]
        [Column(Order = 1)]
        public int RefGradebookPeriodTypeId { get; set; }

        [Display(Name = "Gradebook Period Type Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the gradebook period type")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Period Type", Prompt = "Enter the gradebook period type")]
        [MaxLength(150)]
        [Column(Order = 3)]
        public string? GradebookPeriodType { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 4)]
        public int SortOrder { get; set; }

        public virtual ICollection<GradebookPeriod> GradebookPeriods { get; set; }

    }
}

