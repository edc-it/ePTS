using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Assessments
{
    [Table("AssessmentResult")]
    public class AssessmentResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Result")]
        [Comment("Unique identifier for each assessment result record in the table")]
        [Column(Order = 1)]
        public Guid AssessmentResultId { get; set; }

        [Display(Name = "Gradebook Assessment", Prompt = "Select the gradebook assessment name")]
        [Comment("Reference to the gradebook assessment of a grade, such as grade 1 term 1 week 5. This is a foreign key that references the GradebookAssessment table")]
        [Column(Order = 2)]
        public Guid? GradebookAssessmentId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the assessment result was registered or added to the database")]
        [Column(Order = 3)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Sex", Prompt = "Select the sex")]
        [Comment("A reference to the sex of a learner or teacher, such as male, or female. This is a foreign key that references the RefSex table")]
        [Column(Order = 4)]
        public int? RefSexId { get; set; }

        [Display(Name = "Assessment Category", Prompt = "Select the assessment category")]
        [Comment("A reference to the categories of an assessment, such as fuency, reading comprehension, or phonemic awareness. This is a foreign key that references the RefAssessmentCategory table")]
        [Column(Order = 5)]
        public int? RefAssessmentCategoryId { get; set; }

        [Display(Name = "Assessment Item", Prompt = "Select the assessment item")]
        [Comment("A reference to the item of an assessment category. It represents the specific question items associated with a particular category, such as the fuency category, within a specific grade level. This is a foreign key that references the RefAssessmentItem table")]
        [Column(Order = 6)]
        public int? RefAssessmentItemId { get; set; }

        [Display(Name = "Assessment Result Data Type", Prompt = "Select the assessment result data type")]
        [Comment("A reference to the data type of the assessment result score value. This is a foreign key that references the RefAssessmentResultDataType table")]
        [Column(Order = 7)]
        public int? RefAssessmentResultDataTypeId { get; set; }

        [Display(Name = "Score Metric Type", Prompt = "Select the score metric type")]
        [Comment("The specific method used to report the performance and achievement of the assessment.")]
        [Column(Order = 8)]
        public int? RefScoreMetricTypeId { get; set; }

        [Display(Name = "Possible Value", Prompt = "Enter the possible value")]
        [Column(Order = 9)]
        public int PossibleValue { get; set; }

        [Display(Name = "Score Value", Prompt = "Enter the score value")]
        [Column(Order = 10)]
        public int ScoreValue { get; set; }


    }
}

