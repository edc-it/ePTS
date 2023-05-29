using ePTS.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Reference
{
    [Table("RefAcademicYear")]
    public class RefAcademicYear
    {
        public RefAcademicYear()
        {
            SchoolAcademicYears = new HashSet<SchoolAcademicYear>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Academic Year")]
        [Column(Order = 1)]
        public int RefAcademicYearId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Academic Year", Prompt = "Enter the academic year")]
        [MaxLength(150)]
        [Column(Order = 2)]
        public string? AcademicYear { get; set; } = null!;

        [Display(Name = "Academic Year Code", Prompt = "Enter the academic year code")]
        [MaxLength(100)]
        [Comment("A short code that represents the academic year")]
        [Column(Order = 3)]
        public string? Code { get; set; }

        [Display(Name = "Description", Prompt = "Enter the description")]
        [MaxLength(384)]
        [Column(Order = 4)]
        public string? Description { get; set; }

        [Display(Name = "Start Date", Prompt = "Enter the start date")]
        [Comment("Date on which the academic year starts")]
        [Column(Order = 5)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date", Prompt = "Enter the end date")]
        [Comment("Date on which the academic year ends")]
        [Column(Order = 6)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Academic Year Status", Prompt = "Select the academic year status")]
        [Column(Order = 7)]
        public int? RefAcademicYearStatusId { get; set; }

        [Display(Name = "Sort Order", Prompt = "Enter the sort order")]
        [Column(Order = 8)]
        public int SortOrder { get; set; }

        public virtual ICollection<SchoolAcademicYear> SchoolAcademicYears { get; set; }

    }
}

