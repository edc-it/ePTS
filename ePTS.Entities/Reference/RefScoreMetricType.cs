using ePTS.Entities.Assessments;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefScoreMetricType")]
    public class RefScoreMetricType
    {
        public RefScoreMetricType()
        {
            AssessmentPerformanceLevels = new HashSet<AssessmentPerformanceLevel>();
            AssessmentResults = new HashSet<AssessmentResult>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Score Metric Type")]
        [Comment("The unique identifier for each assessment item type in the table")]
        [Column(Order = 1)]
        public int RefScoreMetricTypeId { get; set; }

        [Display(Name = "Score Metric Type Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the score metric type")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Score Metric Type", Prompt = "Enter the score metric type")]
        [MaxLength(150)]
        [Comment("The name of the assessment item type ")]
        [Column(Order = 3)]
        public string? ScoreMetricType { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Comment("A numeric value that represents the order in which the assessment item type should be displayed")]
        [Column(Order = 4)]
        public int SortOrder { get; set; }

        public virtual ICollection<AssessmentPerformanceLevel> AssessmentPerformanceLevels { get; set; }
        public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }

    }
}

