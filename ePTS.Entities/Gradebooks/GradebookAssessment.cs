using ePTS.Entities.Assessments;
using ePTS.Entities.Reference;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Gradebooks
{
    // Represents a gradebook assessment entity.
    [Table("GradebookAssessment")]
    [Comment("Represents a gradebook assessment entity.")]
    public class GradebookAssessment : BaseEntity
    {
        public GradebookAssessment()
        {
            AssessmentPerformanceLevels = new HashSet<AssessmentPerformanceLevel>();
            AssessmentResults = new HashSet<AssessmentResult>();
        }

        // Unique identifier for each gradebook assessment record in the table.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Assessment")]
        [Comment("Unique identifier for each gradebook assessment record in the table.")]
        [Column(Order = 1)]
        public Guid GradebookAssessmentId { get; set; }

        // Unique identifier of the gradebook to which the assessment aggregate belongs.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook", Prompt = "Select the gradebook")]
        [Comment("Reference to the gradebook to which the assessment aggregate belongs. This is a foreign key that references the Gradebook table.")]
        [Column(Order = 2)]
        public Guid GradebookId { get; set; }

        // Reference to the gradebook assessment period of the gradebook.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Assessment Period", Prompt = "Select the gradebook assessment period")]
        [Comment("A reference to the gradebook assessment period of the gradebook. This is a foreign key that references the GradebookAssessmentPeriod table.")]
        [Column(Order = 3)]
        public Guid GradebookAssessmentPeriodId { get; set; }

        // Registration date of the gradebook assessment.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the gradebook assessment was registered or added to the database.")]
        [DataType(DataType.Date)]
        [Column(Order = 4)]
        public DateTime RegistrationDate { get; set; }

        // Number of females assessed.
        [Display(Name = "Assessed Female", Prompt = "Enter the number of females assessed")]
        [Column(Order = 5)]
        public int AssessedFemale { get; set; } = 0;

        // Number of males assessed.
        [Display(Name = "Assessed Male", Prompt = "Enter the number of males assessed")]
        [Column(Order = 6)]
        public int AssessedMale { get; set; } = 0;

        // Reference to the status of the gradebook assessment.
        [Display(Name = "Gradebook Assessment Status")]
        [Comment("A reference to the status of the gradebook assessment, such as active or inactive. This is a foreign key that references the RefGradebookAssessmentStatus table.")]
        [Column(Order = 11)]
        public int? RefGradebookAssessmentStatusId { get; set; }

        // Indicates whether the assessment results are missing or not.
        [Display(Name = "IsMissingAssessmentResults")]
        [Column(Order = 12)]
        [Comment("Indicates whether the assessment results are missing or not.")]
        public bool? IsMissingAssessmentResults { get; set; }

        // Navigation property referencing the Gradebook entity.
        [ForeignKey("GradebookId")]
        [Display(Name = "Gradebooks")]
        public virtual Gradebook? Gradebooks { get; set; }

        // Navigation property referencing the GradebookAssessmentPeriod entity.
        [ForeignKey("GradebookAssessmentPeriodId")]
        [Display(Name = "Gradebook Assessment Periods")]
        public virtual GradebookAssessmentPeriod? GradebookAssessmentPeriods { get; set; }

        // Navigation property referencing the RefGradebookAssessmentStatus entity.
        [ForeignKey("RefGradebookAssessmentStatusId")]
        [Display(Name = "Gradebook Assessment Status")]
        public virtual RefGradebookAssessmentStatus? GradebookAssessmentStatus { get; set; }

        // Collection navigation property representing the assessment performance levels associated with the gradebook assessment.
        public virtual ICollection<AssessmentPerformanceLevel> AssessmentPerformanceLevels { get; set; }

        // Collection navigation property representing the assessment results associated with the gradebook assessment.
        public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }
    }

}

