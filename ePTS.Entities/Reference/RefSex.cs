using ePTS.Entities.Assessments;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefSex")]
    public class RefSex
    {
        public RefSex()
        {
            AssessmentResults = new HashSet<AssessmentResult>();
            AssessmentPerformanceLevels = new HashSet<AssessmentPerformanceLevel>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Sex")]
        [Comment("The foreign key identifier of the sex of the Person.")]
        [Column(Order = 1)]
        public int RefSexId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Sex", Prompt = "Enter the sex")]
        [MaxLength(50)]
        [Column(Order = 2)]
        public string? Sex { get; set; } = null!;

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Sex Abbrv")]
        [MaxLength(1)]
        [Comment("Abbreviated representation or code used to denote the sex of an individual, such as F for Female")]
        [Column(Order = 3)]
        public string? SexId { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 4)]
        public int SortOrder { get; set; }

        public virtual ICollection<AssessmentResult> AssessmentResults { get; set; }
        public virtual ICollection<AssessmentPerformanceLevel> AssessmentPerformanceLevels { get; set; }

    }
}

