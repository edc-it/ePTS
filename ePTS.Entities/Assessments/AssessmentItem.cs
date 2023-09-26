using ePTS.Entities.Reference;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Assessments
{
    // Represents an assessment item.
    [Table("AssessmentItem")]
    [Comment("Represents an assessment item.")]
    public class AssessmentItem : BaseEntity
    {
        public AssessmentItem()
        {
            AssessmentResults = new HashSet<AssessmentResult>();
        }

        // Unique identifier for each assessment item.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Item", Prompt = "Select the assessment item")]
        [Column(Order = 1)]
        public Guid AssessmentItemId { get; set; }

        // Reference to the assessment to which the item belongs.
        [Display(Name = "Assessment")]
        [Column(Order = 2)]
        public Guid? AssessmentId { get; set; }

        // Reference to the assessment category of the item.
        [Display(Name = "Assessment Category", Prompt = "Select the assessment category")]
        [Column(Order = 3)]
        public int? RefAssessmentCategoryId { get; set; }

        // Assessment item code.
        [Display(Name = "Assessment Item Code")]
        [MaxLength(25)]
        [Column(Order = 4)]
        public string? Code { get; set; }

        // Assessment item number.
        [Display(Name = "Assessment Item Number")]
        [Column(Order = 5)]
        public int? AssessmentItemNumber { get; set; }

        // Assessment item text.
        [Display(Name = "Assessment Item Text")]
        [MaxLength(384)]
        [Column(Order = 6)]
        public string? AssessmentItemText { get; set; }

        // Assessment item description.
        [Display(Name = "Assessment Item Description")]
        [MaxLength(384)]
        [Column(Order = 7)]
        public string? AssessmentItemDescription { get; set; }

        // Maximum score for the assessment item.
        [Display(Name = "Maximum Score")]
        [Column(Order = 8)]
        public double? MaximumScore { get; set; }

        // Minimum score for the assessment item.
        [Display(Name = "Minimum Score")]
        [Column(Order = 9)]
        public double? MinimumScore { get; set; }

        // Sort order of the assessment item.
        [Display(Name = "Sort Order")]
        [Column(Order = 10)]
        public int? SortOrder { get; set; }

        // Navigation property referencing the Assessment entity.
        [ForeignKey("AssessmentId")]
        public virtual Assessment? Assessments { get; set; }

        // Navigation property referencing the RefAssessmentCategory entity.
        [ForeignKey("RefAssessmentCategoryId")]
        public virtual RefAssessmentCategory? AssessmentCategories { get; set; }

        // Collection navigation property for related AssessmentResult entities.
        public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }
    }

}

