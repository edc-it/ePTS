using ePTS.Entities.Gradebooks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefAssessmentPeriod")]
    public class RefAssessmentPeriod
    {
        public RefAssessmentPeriod()
        {
            GradebookAssessments = new HashSet<GradebookAssessment>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Period")]
        [Column(Order = 1)]
        public int RefAssessmentPeriodId { get; set; }

        [Display(Name = "Assessment Period Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the assessment period")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Display(Name = "Assessment Term")]
        [Column(Order = 3)]
        public int? RefAssessmentTermId { get; set; }

        [Display(Name = "Assessment Week")]
        [Column(Order = 4)]
        public int? RefAssessmentWeekId { get; set; }

        [Display(Name = "Start Date", Prompt = "Enter the start date")]
        [Comment("Date on which the assessment period starts")]
        [Column(Order = 5)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date", Prompt = "Enter the end date")]
        [Comment("Date on which the assessment period ends")]
        [Column(Order = 6)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 7)]
        public int SortOrder { get; set; }

        public virtual ICollection<GradebookAssessment> GradebookAssessments { get; set; }

    }
}

