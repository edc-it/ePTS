using ePTS.Entities.Reference;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Gradebooks
{

    // Represents a gradebook assessment period entity.
    [Table("GradebookAssessmentPeriod")]
    [Comment("Represents a gradebook assessment period entity.")]
    public class GradebookAssessmentPeriod : BaseEntity
    {
        public GradebookAssessmentPeriod()
        {
            GradebookAssessments = new HashSet<GradebookAssessment>();
        }

        // Unique identifier for each gradebook assessment period.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Assessment Period")]
        [Comment("Unique identifier for each gradebook assessment period.")]
        [Column(Order = 1)]
        public Guid GradebookAssessmentPeriodId { get; set; }

        // Reference to the gradebook period.
        [Display(Name = "Gradebook Period")]
        [Column(Order = 2)]
        public Guid? GradebookPeriodId { get; set; }

        // Short code that represents the gradebook assessment period.
        [Display(Name = "Gradebook Assessment Period Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the gradebook assessment period.")]
        [Column(Order = 3)]
        public string? Code { get; set; }

        // Name of the gradebook assessment period.
        [Display(Name = "Gradebook Assessment Period Name")]
        [MaxLength(255)]
        [Column(Order = 4)]
        public string? GradebookAssessmentPeriodName { get; set; }

        // Reference to the assessment term.
        [Display(Name = "Assessment Term")]
        [Column(Order = 5)]
        public int? RefAssessmentTermId { get; set; }

        // Reference to the assessment week.
        [Display(Name = "Assessment Week")]
        [Column(Order = 6)]
        public int? RefAssessmentWeekId { get; set; }

        // Start date of the assessment period.
        [Display(Name = "Start Date", Prompt = "Enter the start date")]
        [Comment("Date on which the period starts.")]
        [DataType(DataType.Date)]
        [Column(Order = 7)]
        public DateTime? StartDate { get; set; }

        // End date of the assessment period.
        [Display(Name = "End Date", Prompt = "Enter the end date")]
        [Comment("Date on which the period ends.")]
        [DataType(DataType.Date)]
        [Column(Order = 8)]
        public DateTime? EndDate { get; set; }

        // Sort order of the assessment period.
        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 9)]
        public int? SortOrder { get; set; }

        // Navigation property referencing the GradebookPeriod entity.
        public virtual GradebookPeriod? GradebookPeriods { get; set; }

        // Navigation property referencing the RefAssessmentTerm entity.
        public virtual RefAssessmentTerm? AssessmentTerms { get; set; }

        // Navigation property referencing the RefAssessmentWeek entity.
        public virtual RefAssessmentWeek? AssessmentWeeks { get; set; }

        // Collection navigation property representing the gradebook assessments associated with the assessment period.
        public virtual ICollection<GradebookAssessment> GradebookAssessments { get; set; }
    }

}

