using ePTS.Entities.Assessments;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefAssessmentFormStatus")]
    public class RefAssessmentFormStatus
    {
        public RefAssessmentFormStatus()
        {
            AssessmentForms = new HashSet<AssessmentForm>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Form Status")]
        [Comment("The unique identifier for each assessment form status in the table")]
        [Column(Order = 1)]
        public int RefAssessmentFormStatusId { get; set; }

        [Display(Name = "Assessment Form Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the assessment form")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Form Status", Prompt = "Enter the assessment form status")]
        [MaxLength(150)]
        [Comment("The name of the assessment form status ")]
        [Column(Order = 3)]
        public string? AssessmentFormStatus { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Comment("A numeric value that represents the order in which the assessment form status should be displayed")]
        [Column(Order = 4)]
        public int SortOrder { get; set; }

        public virtual ICollection<AssessmentForm> AssessmentForms { get; set; }

    }
}

