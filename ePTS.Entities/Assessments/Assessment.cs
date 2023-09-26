using ePTS.Entities.Gradebooks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Assessments
{
    // Represents an assessment record.
    [Table("Assessment")]
    [Comment("Represents an assessment record.")]
    public class Assessment : BaseEntity
    {
        // Unique identifier for each assessment record in the table.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment")]
        [Comment("Unique identifier for each assessment record in the table.")]
        [Column(Order = 1)]
        public Guid AssessmentId { get; set; }

        // Date on which the assessment was registered or added to the database.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the assessment was registered or added to the database.")]
        [DataType(DataType.Date)]
        [Column(Order = 2)]
        public DateTime RegistrationDate { get; set; }

        // Name of the assessment.
        [Display(Name = "Assessment Name", Prompt = "Enter the assessment name")]
        [MaxLength(255)]
        [Column(Order = 3)]
        public string? AssessmentName { get; set; }

        // Short code that represents the assessment.
        [Display(Name = "Assessment Code", Prompt = "Enter the assessment code")]
        [MaxLength(100)]
        [Comment("A short code that represents the assessment.")]
        [Column(Order = 4)]
        public string? Code { get; set; }

        // Reference to the type of assessment.
        [Display(Name = "Assessment Type", Prompt = "Select the assessment type")]
        [Comment("A reference to the type of assessment. This is a foreign key that references the RefAssessmentType table.")]
        [Column(Order = 5)]
        public int? RefAssessmentTypeId { get; set; }

        // Reference to the status of assessment.
        [Display(Name = "Assessment Status", Prompt = "Select the assessment status")]
        [Comment("A reference to the status of assessment. This is a foreign key that references the RefAssessmentStatus table.")]
        [Column(Order = 6)]
        public int? RefAssessmentStatusId { get; set; }

        // Reference to the grade level of the assessment.
        [Display(Name = "Grade Level")]
        [Comment("A reference to the grade level of the assessment. This is a foreign key that references the GradeLevel table.")]
        [Column(Order = 7)]
        public int? RefGradeLevelId { get; set; }

        // Navigation property referencing the Gradebook entity.
        public virtual ICollection<Gradebook> Gradebooks { get; set; }

        // Navigation property referencing the AssessmentItem entity.
        public virtual ICollection<AssessmentItem> AssessmentItems { get; set; }
    }
}

