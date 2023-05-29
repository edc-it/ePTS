using ePTS.Entities.Gradebooks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefAssessmentWeek")]
    public class RefAssessmentWeek
    {
        public RefAssessmentWeek()
        {
            AssessmentPeriods = new HashSet<RefAssessmentPeriod>();
            GradebookPeriodForms = new HashSet<GradebookPeriodForm>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Week")]
        [Comment("The unique identifier for each assessment item type in the table")]
        [Column(Order = 1)]
        public int RefAssessmentWeekId { get; set; }

        [Display(Name = "Assessment Week Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the assessment week")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Week")]
        [MaxLength(150)]
        [Comment("The name of the assessment item type ")]
        [Column(Order = 3)]
        public string? AssessmentWeek { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Comment("A numeric value that represents the order in which the assessment item type should be displayed")]
        [Column(Order = 4)]
        public int SortOrder { get; set; }

        public virtual ICollection<RefAssessmentPeriod> AssessmentPeriods { get; set; }
        public virtual ICollection<GradebookPeriodForm> GradebookPeriodForms { get; set; }

    }
}

