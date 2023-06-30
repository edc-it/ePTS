using ePTS.Entities.Reference;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Assessments
{
    [Table("AssessmentItem")]
    public class AssessmentItem : BaseEntity
    {
        public AssessmentItem()
        {
            AssessmentResults = new HashSet<AssessmentResult>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Item", Prompt = "Select the assessment item")]
        [Column(Order = 1)]
        public Guid AssessmentItemId { get; set; }

        [Display(Name = "Assessment")]
        [Column(Order = 2)]
        public Guid? AssessmentId { get; set; }

        [Display(Name = "Assessment Category", Prompt = "Select the assessment category")]
        [Column(Order = 3)]
        public int? RefAssessmentCategoryId { get; set; }

        [Display(Name = "Assessment Item Code")]
        [MaxLength(25)]
        [Column(Order = 4)]
        public string? Code { get; set; }

        [Display(Name = "Assessment Item Number")]
        [Column(Order = 5)]
        public int? AssessmentItemNumber { get; set; }

        [Display(Name = "Assessment Item Text")]
        [MaxLength(384)]
        [Column(Order = 6)]
        public string? AssessmentItemText { get; set; }

        [Display(Name = "Assessment Item Description")]
        [MaxLength(384)]
        [Column(Order = 7)]
        public string? AssessmentItemDescription { get; set; }

        [Display(Name = "Maximum Score")]
        [Column(Order = 8)]
        public double? MaximumScore { get; set; }

        [Display(Name = "Minimum Score")]
        [Column(Order = 9)]
        public double? MinimumScore { get; set; }

        [Display(Name = "Sort Order")]
        [Column(Order = 10)]
        public int? SortOrder { get; set; }

        [ForeignKey("AssessmentId")]
        public virtual Assessment? Assessments { get; set; }

        [ForeignKey("RefAssessmentCategoryId")]
        public virtual RefAssessmentCategory? AssessmentCategories { get; set; }

        public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }

    }
}

