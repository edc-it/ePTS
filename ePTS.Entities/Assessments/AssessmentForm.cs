using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Assessments
{
    [Table("AssessmentForm")]
    public class AssessmentForm
    {
        public AssessmentForm()
        {
            AssessmentFormItems = new HashSet<AssessmentFormItem>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Form")]
        [Comment("Unique identifier for each assessment form record in the table")]
        [Column(Order = 1)]
        public Guid AssessmentFormId { get; set; }

        [Display(Name = "Assessment")]
        [Comment("Reference to the assessment to which the assessment form belongs. This is a foreign key that references the Assessment table")]
        [Column(Order = 2)]
        public Guid? AssessmentId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the assessment form was registered or added to the database")]
        [Column(Order = 3)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Assessment Form Name", Prompt = "Enter the assessment form name")]
        [MaxLength(255)]
        [Column(Order = 4)]
        public string? AssessmentFormName { get; set; }

        [Display(Name = "Assessment Form Code", Prompt = "Enter the assessment form code")]
        [MaxLength(100)]
        [Comment("A short code that represents the assessment form")]
        [Column(Order = 5)]
        public string? Code { get; set; }

        [Display(Name = "Assessment Form Status", Prompt = "Select the assessment form status")]
        [Column(Order = 6)]
        public int? RefAssessmentFormStatusId { get; set; }

        public virtual ICollection<AssessmentFormItem> AssessmentFormItems { get; set; }

    }
}

