using ePTS.Entities.Assessments;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefAssessmentCategory")]
    public class RefAssessmentCategory
    {
        public RefAssessmentCategory()
        {
            AssessmentItems = new HashSet<AssessmentItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Category")]
        [Comment("The unique identifier for each assessment category in the table")]
        [Column(Order = 1)]
        public int RefAssessmentCategoryId { get; set; }

        [Display(Name = "Assessment Category Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the assessment category")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Category", Prompt = "Enter the assessment category")]
        [MaxLength(150)]
        [Comment("The name of the assessment category")]
        [Column(Order = 3)]
        public string? AssessmentCategory { get; set; } = null!;

        [Display(Name = "Assessment Category Description", Prompt = "Enter the assessment category description")]
        [MaxLength(384)]
        [Comment("The description of the assessment category")]
        [Column(Order = 4)]
        public string? AssessmentCategoryDescription { get; set; } = null!;

        //Foreign Key for RefGradeLevelId
        [Display(Name = "Grade Level", Prompt = "Select the grade level")]
        [Comment("The grade level for the assessment category")]
        [Column(Order = 5)]
        public int? RefGradeLevelId { get; set; }

        [ForeignKey("RefGradeLevelId")]
        public virtual RefGradeLevel? RefGradeLevel { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Comment("A numeric value that represents the order in which the assessment categories should be displayed")]
        [Column(Order = 6)]
        public int? SortOrder { get; set; }

        public virtual ICollection<AssessmentItem> AssessmentItems { get; set; }

    }
}

