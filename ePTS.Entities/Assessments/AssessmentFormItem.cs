using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Assessments
{
    [Table("AssessmentFormItem")]
    public class AssessmentFormItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Form Item", Prompt = "Select the assessment item")]
        [Column(Order = 1)]
        public Guid AssessmentFormItemId { get; set; }

        [Display(Name = "Assessment Form")]
        [Column(Order = 2)]
        public Guid? AssessmentFormId { get; set; }

        [Display(Name = "Assessment Category", Prompt = "Select the assessment category")]
        [Column(Order = 3)]
        public int? RefAssessmentCategoryId { get; set; }

        [Display(Name = "Assessment Form Item Number")]
        [MaxLength(25)]
        [Column(Order = 4)]
        public string? AssessmentFormItemNumber { get; set; }

        [Display(Name = "Assessment Form Item Text")]
        [MaxLength(384)]
        [Column(Order = 5)]
        public string? AssessmentFormItemText { get; set; }

        [Display(Name = "Maximum Score")]
        [Column(Order = 6)]
        public int MaximumScore { get; set; }

        [Display(Name = "Minimum Score")]
        [Column(Order = 7)]
        public int MinimumScore { get; set; }

        [Display(Name = "Sort Order")]
        [Column(Order = 8)]
        public int SortOrder { get; set; }


    }
}

