using ePTS.Entities.Assessments;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Gradebooks
{
    [Table("GradebookAssessment")]
    public class GradebookAssessment
    {
        public GradebookAssessment()
        {
            AssessmentPerformanceLevels = new HashSet<AssessmentPerformanceLevel>();
            AssessmentResults = new HashSet<AssessmentResult>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Assessment")]
        [Comment("Unique identifier for each gradebook assessment record in the table")]
        [Column(Order = 1)]
        public Guid GradebookAssessmentId { get; set; }

        [Display(Name = "Gradebook", Prompt = "Select the gradebook")]
        [Comment("Reference to the gradebook to which the assessment aggregate belongs to. This is a foreign key that references the Gradebook table")]
        [Column(Order = 2)]
        public Guid? GradebookId { get; set; }

        [Display(Name = "Assessment Period", Prompt = "Select the assessment period")]
        [Comment("A reference to the assessment period of the gradebook. This is a foreign key that references the AssessmentPeriod table")]
        [Column(Order = 3)]
        public int? RefAssessmentPeriodId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the gradebook assessment was registered or added to the database")]
        [Column(Order = 4)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "AssessedFemale", Prompt = "Enter the number of females assessed")]
        [Column(Order = 5)]
        public int AssessedFemale { get; set; }

        [Display(Name = "AssessedMale", Prompt = "Enter the number of males assessed")]
        [Column(Order = 6)]
        public int AssessedMale { get; set; }

        [Display(Name = "EnrolledFemale", Prompt = "Enter the number of females enrolled")]
        [Column(Order = 7)]
        public int EnrolledFemale { get; set; }

        [Display(Name = "EnrolledMale", Prompt = "Enter the number of males enrolled")]
        [Column(Order = 8)]
        public int EnrolledMale { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "IsMissingAssessmentResults", Prompt = "Are assessment results missing? (Yes/No)")]
        [Column(Order = 9)]
        public bool IsMissingAssessmentResults { get; set; }

        [Display(Name = "Gradebook Assessment Status")]
        [Comment("A reference to the status of the gradebook, such as active or inactive. This is a foreign key that references the RefGradebookStatus table")]
        [Column(Order = 10)]
        public int? RefGradebookAssessmentStatusId { get; set; }

        public virtual ICollection<AssessmentPerformanceLevel> AssessmentPerformanceLevels { get; set; }
        public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }

    }
}

