using ePTS.Entities.Assessments;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefAssessmentType")]
    public class RefAssessmentType
    {
        public RefAssessmentType()
        {
            Assessments = new HashSet<Assessment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Type")]
        [Column(Order = 1)]
        public int RefAssessmentTypeId { get; set; }

        [Display(Name = "Assessment Type Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the assessment type")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Type", Prompt = "Enter the assessment type")]
        [MaxLength(150)]
        [Column(Order = 3)]
        public string? AssessmentType { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 4)]
        public int? SortOrder { get; set; }

        public virtual ICollection<Assessment> Assessments { get; set; }

    }
}

