using ePTS.Entities.Assessments;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefAssessmentResultDataType")]
    public class RefAssessmentResultDataType
    {
        public RefAssessmentResultDataType()
        {
            AssessmentPerformanceLevels = new HashSet<AssessmentPerformanceLevel>();
            AssessmentResults = new HashSet<AssessmentResult>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Item Type")]
        [Comment("The unique identifier for each assessment item type in the table")]
        [Column(Order = 1)]
        public int RefAssessmentItemTypeId { get; set; }

        [Display(Name = "Assessment Result Data Type Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the assessment result type")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Item Type")]
        [MaxLength(150)]
        [Comment("The name of the assessment item type ")]
        [Column(Order = 3)]
        public string? AssessmentItemType { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Comment("A numeric value that represents the order in which the assessment item type should be displayed")]
        [Column(Order = 4)]
        public int SortOrder { get; set; }

        public virtual ICollection<AssessmentPerformanceLevel> AssessmentPerformanceLevels { get; set; }
        public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }

    }
}

