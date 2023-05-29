using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Gradebooks
{
    [Table("GradebookPeriodForm")]
    public class GradebookPeriodForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Gradebook Period Form")]
        [Column(Order = 1)]
        public Guid GradebookPeriodFormId { get; set; }

        [Display(Name = "Gradebook Period")]
        [Column(Order = 2)]
        public Guid? GradebookPeriodId { get; set; }

        [Display(Name = "Gradebook Period Form Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the gradebook period form")]
        [Column(Order = 3)]
        public string? Code { get; set; }

        [Display(Name = "Gradebook Period Name")]
        [MaxLength(255)]
        [Column(Order = 4)]
        public string? GradebookPeriodName { get; set; }

        [Display(Name = "Assessment Term")]
        [Column(Order = 5)]
        public int? RefAssessmentTermId { get; set; }

        [Display(Name = "Assessment Week")]
        [Column(Order = 6)]
        public int? RefAssessmentWeekId { get; set; }


    }
}

