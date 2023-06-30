using ePTS.Entities.Gradebooks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Assessments
{
    [Table("Assessment")]
    public class Assessment : BaseEntity
    {
        public Assessment()
        {
            Gradebooks = new HashSet<Gradebook>();
            AssessmentItems = new HashSet<AssessmentItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment")]
        [Comment("Unique identifier for each assessment record in the table")]
        [Column(Order = 1)]
        public Guid AssessmentId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the assessment was registered or added to the database")]
        [DataType(DataType.Date)]
        [Column(Order = 2)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Assessment Name", Prompt = "Enter the assessment name")]
        [MaxLength(255)]
        [Column(Order = 3)]
        public string? AssessmentName { get; set; }

        [Display(Name = "Assessment Code", Prompt = "Enter the assessment code")]
        [MaxLength(100)]
        [Comment("A short code that represents the assessment")]
        [Column(Order = 4)]
        public string? Code { get; set; }

        [Display(Name = "Assessment Type", Prompt = "Select the assessment type")]
        [Comment("A reference to the type of assessment. This is a foreign key that references the RefAssessmentType table")]
        [Column(Order = 5)]
        public int? RefAssessmentTypeId { get; set; }

        [Display(Name = "Assessment Status", Prompt = "Select the assessment status")]
        [Comment("A reference to the status of assessment. This is a foreign key that references the RefAssessmentStatus table")]
        [Column(Order = 6)]
        public int? RefAssessmentStatusId { get; set; }

        [Display(Name = "Grade Level")]
        [Comment("A reference to the grade level of the assessment. This is a foreign key that references the GradeLevel table")]
        [Column(Order = 7)]
        public int? RefGradeLevelId { get; set; }

        public virtual ICollection<Gradebook> Gradebooks { get; set; }
        public virtual ICollection<AssessmentItem> AssessmentItems { get; set; }

    }
}

