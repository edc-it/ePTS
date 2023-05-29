using ePTS.Entities.Assessments;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefAssessmentStatus")]
    public class RefAssessmentStatus
    {
        public RefAssessmentStatus()
        {
            Assessments = new HashSet<Assessment>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Status")]
        [Comment("The unique identifier for each assessment item type in the table")]
        [Column(Order = 1)]
        public int RefAssessmentStatusId { get; set; }

        [Display(Name = "Assessment Status Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the assessment status")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Status", Prompt = "Enter the assessment status")]
        [MaxLength(150)]
        [Comment("The name of the assessment item type ")]
        [Column(Order = 3)]
        public string? AssessmentStatus { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Comment("A numeric value that represents the order in which the assessment item type should be displayed")]
        [Column(Order = 4)]
        public int SortOrder { get; set; }

        public virtual ICollection<Assessment> Assessments { get; set; }

    }
}

