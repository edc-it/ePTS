using ePTS.Entities.Gradebooks;
using ePTS.Entities.Reference;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Assessments
{
    // Represents an assessment result record.
    [Table("AssessmentResult")]
    [Comment("Represents an assessment result record.")]
    public class AssessmentResult : BaseEntity
    {
        // Unique identifier for each assessment result record in the table.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Result")]
        [Comment("Unique identifier for each assessment result record in the table.")]
        [Column(Order = 1)]
        public Guid AssessmentResultId { get; set; }

        // Reference to the gradebook assessment of a grade.
        [Display(Name = "Gradebook Assessment", Prompt = "Select the gradebook assessment name")]
        [Comment("Reference to the gradebook assessment of a grade, such as grade 1 term 1 week 5. This is a foreign key that references the GradebookAssessment table.")]
        [Column(Order = 2)]
        public Guid GradebookAssessmentId { get; set; }

        // Date on which the assessment result was registered or added to the database.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the assessment result was registered or added to the database.")]
        [DataType(DataType.Date)]
        [Column(Order = 3)]
        public DateTime RegistrationDate { get; set; }

        // Reference to the item of an assessment category.
        [Display(Name = "Assessment Item", Prompt = "Select the assessment item")]
        [Comment("A reference to the item of an assessment category. It represents the specific question items associated with a particular category, such as the fluency category, within a specific grade level. This is a foreign key that references the RefAssessmentItem table.")]
        [Column(Order = 4)]
        public Guid AssessmentItemId { get; set; }

        // Represents the count of accurate responses provided by female participants.
        [Display(Name = "Female", Prompt = "Enter the total number of correct responses from female participants")]
        [Comment("Represents the count of accurate responses provided by female participants.")]
        [Column(Order = 5)]
        public int? ScoreFemale { get; set; }

        // Represents the count of accurate responses provided by male participants.
        [Display(Name = "Male", Prompt = "Enter the total number of correct responses from male participants")]
        [Comment("Represents the count of accurate responses provided by male participants.")]
        [Column(Order = 6)]
        public int? ScoreMale { get; set; }

        // The cumulative number of correct responses from all participants.
        [Display(Name = "Total Value", Prompt = "Enter the total number of correct responses from all participants")]
        [Comment("The cumulative number of correct responses from all participants.")]
        [Column(Order = 7)]
        public int? Score { get; set; }

        // Navigation property referencing the GradebookAssessment entity.
        [ForeignKey("GradebookAssessmentId")]
        [Display(Name = "Gradebook Assessments")]
        public virtual GradebookAssessment? GradebookAssessments { get; set; }

        // Navigation property referencing the AssessmentItem entity.
        [ForeignKey("AssessmentItemId")]
        [Display(Name = "Assessment Items")]
        public virtual AssessmentItem? AssessmentItems { get; set; }
    }

}

