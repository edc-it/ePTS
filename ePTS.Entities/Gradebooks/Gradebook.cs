using ePTS.Entities.Assessments;
using ePTS.Entities.Core;
using ePTS.Entities.Reference;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Gradebooks
{
    [Table("Gradebook")]
    public class Gradebook : BaseEntity
    {
        public Gradebook()
        {
            GradebookAssessments = new HashSet<GradebookAssessment>();
            GradebookEnrollments = new HashSet<GradebookEnrollment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook")]
        [Comment("Unique identifier for each gradebook")]
        [Column(Order = 1)]
        public Guid GradebookId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Academic Year", Prompt = "Select the academic year")]
        [Comment("Reference to the academic year to which the gradebook belongs. This is a foreign key that references the SchoolAcademicYear table")]
        [Column(Order = 2)]
        public Guid SchoolAcademicYearId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Grade Level", Prompt = "Select the grade level")]
        [Comment("A reference to the grade level of the gradebook. This is a foreign key that references the GradeLevel table")]
        [Column(Order = 3)]
        public int RefGradeLevelId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the gradebook was registered or added to the database")]
        [DataType(DataType.Date)]
        [Column(Order = 4)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Gradebook Period")]
        [Comment("A reference to the default gradebook period for a grade school, such as term 1, term 2, or term 3. This is a foreign key that references the SchoolType table")]
        [Column(Order = 5)]
        public Guid? GradebookPeriodId { get; set; }

        [Display(Name = "Assessment", Prompt = "Select the assessment for this gradebook")]
        [Comment("Reference to the default assessment form for a grade. It includes assessments and their associated items. This is a foreign key that references the Assessment table")]
        [Column(Order = 6)]
        public Guid? AssessmentId { get; set; }

        [Display(Name = "Assessment Platform Type", Prompt = "Select the assessment platform type")]
        [Comment("A reference to the type of platform for the assessment, such as web or android. This is a foreign key that references the RefAssessmentPlatformType table")]
        [Column(Order = 7)]
        public int? RefAssessmentPlatformTypeId { get; set; }

        [Display(Name = "Gradebook Status", Prompt = "Select the gradebook status")]
        [Comment("A reference to the status of a gradebook, such as active or inactive. This is a foreign key that references the RefGradebookStatus table")]
        [Column(Order = 8)]
        public int? RefGradebookStatusId { get; set; }

        [Display(Name = "Is Missing Assessments")]
        [Column(Order = 9)]
        [Comment("Indicates whether the gradebook is missing assessments")]
        public bool? IsMissingGradebookAssessments { get; set; } = null!;

        // Foreign Keys
        [ForeignKey("AssessmentId")]
        [Display(Name = "Assessments")]
        public virtual Assessment? Assessments { get; set; }

        [ForeignKey("GradebookPeriodId")]
        [Display(Name = "Gradebook Periods")]
        public virtual GradebookPeriod? GradebookPeriods { get; set; }

        [ForeignKey("RefAssessmentPlatformTypeId")]
        [Display(Name = "Assessment Platform Types")]
        public virtual RefAssessmentPlatformType? AssessmentPlatformTypes { get; set; }

        [ForeignKey("RefGradebookStatusId")]
        [Display(Name = "Gradebook Status")]
        public virtual RefGradebookStatus? GradebookStatus { get; set; }

        [ForeignKey("RefGradeLevelId")]
        [Display(Name = "Grade Levels")]
        public virtual RefGradeLevel? GradeLevels { get; set; }

        [ForeignKey("SchoolAcademicYearId")]
        [Display(Name = "Academic Year")]
        public virtual SchoolAcademicYear? SchoolAcademicYears { get; set; }

        public virtual ICollection<GradebookAssessment> GradebookAssessments { get; set; }
        public virtual ICollection<GradebookEnrollment> GradebookEnrollments { get; set; }
    }
}

