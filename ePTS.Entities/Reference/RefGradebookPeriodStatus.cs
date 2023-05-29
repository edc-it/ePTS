using ePTS.Entities.Gradebooks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefGradebookPeriodStatus")]
    public class RefGradebookPeriodStatus
    {
        public RefGradebookPeriodStatus()
        {
            GradebookPeriods = new HashSet<GradebookPeriod>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Period Status")]
        [Column(Order = 1)]
        public int RefGradebookPeriodStatusId { get; set; }

        [Display(Name = "Gradebook Period Status Code")]
        [MaxLength(100)]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Period Status", Prompt = "Enter the gradebook period status")]
        [MaxLength(150)]
        [Column(Order = 3)]
        public string? GradebookPeriodStatus { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 4)]
        public int SortOrder { get; set; }

        public virtual ICollection<GradebookPeriod> GradebookPeriods { get; set; }

    }
}

