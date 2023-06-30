using ePTS.Entities.Gradebooks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefGradebookAssessmentStatus")]
    public class RefGradebookAssessmentStatus
    {
        public RefGradebookAssessmentStatus()
        {
            GradebookAssessments = new HashSet<GradebookAssessment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Assessment Status")]
        [Comment("The unique identifier for each assessment item type in the table")]
        [Column(Order = 1)]
        public int RefGradebookAssessmentStatusId { get; set; }

        [Display(Name = "Gradebook Assessment Status Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the gradebook assessment status")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Assessment Status")]
        [MaxLength(150)]
        [Comment("The name of the assessment item type ")]
        [Column(Order = 3)]
        public string? GradebookAssessmentStatus { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Comment("A numeric value that represents the order in which the assessment item type should be displayed")]
        [Column(Order = 4)]
        public int? SortOrder { get; set; }

        public virtual ICollection<GradebookAssessment> GradebookAssessments { get; set; }

    }
}

