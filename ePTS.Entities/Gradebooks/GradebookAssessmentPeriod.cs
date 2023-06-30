using ePTS.Entities.Reference;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Gradebooks
{
    
    [Table("GradebookAssessmentPeriod")]
    public class GradebookAssessmentPeriod : BaseEntity
    {
        public GradebookAssessmentPeriod()
        {
            GradebookAssessments = new HashSet<GradebookAssessment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Assessment Period")]
        [Column(Order = 1)]
        public Guid GradebookAssessmentPeriodId { get; set; }

        [Display(Name = "Gradebook Period")]
        [Column(Order = 2)]
        public Guid? GradebookPeriodId { get; set; }

        [Display(Name = "Gradebook Assessment Period Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the gradebook assessment period")]
        [Column(Order = 3)]
        public string? Code { get; set; }

        [Display(Name = "Gradebook Assessment Period Name")]
        [MaxLength(255)]
        [Column(Order = 4)]
        public string? GradebookAssessmentPeriodName { get; set; }

        [Display(Name = "Assessment Term")]
        [Column(Order = 5)]
        public int? RefAssessmentTermId { get; set; }

        [Display(Name = "Assessment Week")]
        [Column(Order = 6)]
        public int? RefAssessmentWeekId { get; set; }

        [Display(Name = "Start Date", Prompt = "Enter the start date")]
        [Comment("Date on which the period starts")]
        [DataType(DataType.Date)]
        [Column(Order = 7)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date", Prompt = "Enter the end date")]
        [Comment("Date on which the period ends")]
        [DataType(DataType.Date)]
        [Column(Order = 8)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 9)]
        public int? SortOrder { get; set; }

        // Foreign Keys
        [ForeignKey("GradebookPeriodId")]
        [Display(Name = "Gradebook Periods")]
        public virtual GradebookPeriod? GradebookPeriods { get; set; }

        [ForeignKey("RefAssessmentTermId")]
        [Display(Name = "Assessment Terms")]
        public virtual RefAssessmentTerm? AssessmentTerms { get; set; }

        [ForeignKey("RefAssessmentWeekId")]
        [Display(Name = "Assessment Weeks")]
        public virtual RefAssessmentWeek? AssessmentWeeks { get; set; }

        public virtual ICollection<GradebookAssessment> GradebookAssessments { get; set; }

    }
}

