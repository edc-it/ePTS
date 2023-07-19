using ePTS.Entities.Assessments;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefPerformanceLevel")]
    public class RefPerformanceLevel
    {
        public RefPerformanceLevel()
        {
            AssessmentPerformanceLevels = new HashSet<AssessmentPerformanceLevel>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Performance Level")]
        [Comment("The unique identifier for each assessment item type in the table")]
        [Column(Order = 1)]
        public int RefPerformanceLevelId { get; set; }

        [Display(Name = "Performance Level Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the performance level")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Performance Level")]
        [MaxLength(150)]
        [Comment("The name of the assessment item type ")]
        [Column(Order = 3)]
        public string? PerformanceLevel { get; set; } = null!;

        [Display(Name = "Min Performance Level", Prompt = "Enter the minimum performance level")]
        [Comment("The minimum performance level")]
        [Column(Order = 4)]
        public double? MinPerformanceLevel { get; set; }

        [Display(Name = "Max Performance Level", Prompt = "Enter the maximum performance level")]
        [Comment("The maximum performance level")]
        [Column(Order = 5)]
        public double? MaxPerformanceLevel { get; set; }

        public string? PerformanceLevelText { get; set; }

        [Display(Name = "Color", Prompt = "Set the performance level color")]
        [Comment("The color for the performance level")]
        [Column(Order = 6)]
        public string? Color { get; set; }

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Comment("A numeric value that represents the order in which the assessment item type should be displayed")]
        [Column(Order = 7)]
        public int? SortOrder { get; set; }

        public virtual ICollection<AssessmentPerformanceLevel> AssessmentPerformanceLevels { get; set; }

    }
}

