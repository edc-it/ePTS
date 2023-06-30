using ePTS.Entities.Assessments;
using ePTS.Entities.Reference;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Gradebooks
{
    [Table("GradebookAssessment")]
    public class GradebookAssessment : BaseEntity
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

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook", Prompt = "Select the gradebook")]
        [Comment("Reference to the gradebook to which the assessment aggregate belongs to. This is a foreign key that references the Gradebook table")]
        [Column(Order = 2)]
        public Guid GradebookId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Assessment Period", Prompt = "Select the gradebook assessment period")]
        [Comment("A reference to the gradebook assessment period of the gradebook. This is a foreign key that references the GradebookAssessmentPeriod table")]
        [Column(Order = 3)]
        public Guid GradebookAssessmentPeriodId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the gradebook assessment was registered or added to the database")]
        [DataType(DataType.Date)]
        [Column(Order = 4)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "AssessedFemale", Prompt = "Enter the number of females assessed")]
        [Column(Order = 5)]
        public int? AssessedFemale { get; set; }

        [Display(Name = "AssessedMale", Prompt = "Enter the number of males assessed")]
        [Column(Order = 6)]
        public int? AssessedMale { get; set; }

        [Display(Name = "Assessed", Prompt = "Enter the total number assessed")]
        [Column(Order = 7)]
        public int? Assessed { get; set; }

        [Display(Name = "Absent Female", Prompt = "Enter the number of absent females")]
        [Column(Order = 8)]
        public int? AbsentFemale { get; set; }

        [Display(Name = "Absent Male", Prompt = "Enter the number of absent males")]
        [Column(Order = 9)]
        public int? AbsentMale { get; set; }

        [Display(Name = "Assessed", Prompt = "Enter the total number absent")]
        [Column(Order = 10)]
        public int? Absent { get; set; }

        [Display(Name = "Gradebook Assessment Status")]
        [Comment("A reference to the status of the gradebook, such as active or inactive. This is a foreign key that references the RefGradebookStatus table")]
        [Column(Order = 11)]
        public int? RefGradebookAssessmentStatusId { get; set; }

        [Display(Name = "IsMissingAssessmentResults")]
        [Column(Order = 12)]
        [Comment("Indicates whether the assessment results are missing or not")]
        public bool? IsMissingAssessmentResults { get; set; }

        // Foreign Keys
        [ForeignKey("GradebookId")]
        [Display(Name = "Gradebooks")]
        public virtual Gradebook? Gradebooks { get; set; }

        [ForeignKey("GradebookAssessmentPeriodId")]
        [Display(Name = "Gradebook Assessment Periods")]
        public virtual GradebookAssessmentPeriod? GradebookAssessmentPeriods { get; set; }

        [ForeignKey("RefGradebookAssessmentStatusId")]
        [Display(Name = "Gradebook Assessment Status")]
        public virtual RefGradebookAssessmentStatus? GradebookAssessmentStatus { get; set; }

        public virtual ICollection<AssessmentPerformanceLevel> AssessmentPerformanceLevels { get; set; }
        public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }

    }
}

