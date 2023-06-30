using ePTS.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefAcademicYearStatus")]
    public class RefAcademicYearStatus
    {
        public RefAcademicYearStatus()
        {
            AcademicYears = new HashSet<RefAcademicYear>();
            SchoolAcademicYears = new HashSet<SchoolAcademicYear>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Academic Year Status")]
        [Comment("The unique identifier for each academic year status in the table")]
        [Column(Order = 1)]
        public int RefAcademicYearStatusId { get; set; }

        [Display(Name = "Academic Year Status Code")]
        [MaxLength(100)]
        [Comment("A short code that represents the academic year status")]
        [Column(Order = 2)]
        public string? Code { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Academic Year Status", Prompt = "Enter the academic year status")]
        [MaxLength(150)]
        [Comment("The name of the academic year status")]
        [Column(Order = 3)]
        public string? AcademicYearStatus { get; set; } = null!;

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Comment("A numeric value that represents the order in which the academic year statuses should be displayed")]
        [Column(Order = 4)]
        public int? SortOrder { get; set; }

        public virtual ICollection<RefAcademicYear> AcademicYears { get; set; }
        public virtual ICollection<SchoolAcademicYear> SchoolAcademicYears { get; set; }

    }
}

