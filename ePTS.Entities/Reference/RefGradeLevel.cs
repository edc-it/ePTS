using ePTS.Entities.Assessments;
using ePTS.Entities.Gradebooks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefGradeLevel")]
    public class RefGradeLevel
    {
        public RefGradeLevel()
        {
            Assessments = new HashSet<Assessment>();
            Gradebooks = new HashSet<Gradebook>();
            GradebookPeriods = new HashSet<GradebookPeriod>();
            AssessmentCategories = new HashSet<RefAssessmentCategory>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Grade Level")]
        [Column(Order = 1)]
        public int RefGradeLevelId { get; set; }

        [Display(Name = "Grade Level Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the grade level")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Grade Level", Prompt = "Enter the grade level")]
        [MaxLength(150)]
        [Column(Order = 3)]
        public string? GradeLevel { get; set; } = null!;

        [Display(Name = "Grade Level Abbrv")]
        [MaxLength(10)]
        [Comment("Abbreviated representation or code used to denote different educational or academic levels, such as G1 for Grade 1, or P1 for Primary 1")]
        [Column(Order = 4)]
        public string? GradeLevelId { get; set; }

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 5)]
        public int? SortOrder { get; set; }

        public virtual ICollection<Assessment> Assessments { get; set; }
        public virtual ICollection<Gradebook> Gradebooks { get; set; }
        public virtual ICollection<GradebookPeriod> GradebookPeriods { get; set; }
        public virtual ICollection<RefAssessmentCategory> AssessmentCategories { get; set; }

    }
}

