using ePTS.Entities.Gradebooks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Core
{
    [Table("SchoolAcademicYear")]
    public class SchoolAcademicYear
    {
        public SchoolAcademicYear()
        {
            Gradebooks = new HashSet<Gradebook>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Academic Year")]
        [Comment("Unique identifier for each academic year record")]
        [Column(Order = 1)]
        public Guid SchoolAcademicYearId { get; set; }

        [Display(Name = "School")]
        [Comment("Unique identifier of the school that the academic year belongs to. This is a foreign key that references the School table")]
        [Column(Order = 2)]
        public Guid? OrganizationId { get; set; }

        [Display(Name = "Academic Year", Prompt = "Select the academic year")]
        [Comment("A reference to the academic year, such as 2022, 2023, etc. This is a foreign key that references the RefAcademicYear table")]
        [Column(Order = 3)]
        public int? RefAcademicYearId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the school academic year was registered or added to the database")]
        [Column(Order = 4)]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Start Date", Prompt = "Enter the start date")]
        [Comment("the starting date for the academic year")]
        [Column(Order = 5)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "End Date", Prompt = "Enter the end date")]
        [Comment("The ending date for the academic year")]
        [Column(Order = 6)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Status")]
        [Comment("A reference to the status of the academic year, such as started, not started, completed, or closed. This is a foreign key that references the RefAcademicYearStatus table")]
        [Column(Order = 7)]
        public int? RefAcademicYearStatusId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Missing enrollment?")]
        [Column(Order = 8)]
        public bool IsMissingEnrollment { get; set; }

        public virtual ICollection<Gradebook> Gradebooks { get; set; }

    }
}

