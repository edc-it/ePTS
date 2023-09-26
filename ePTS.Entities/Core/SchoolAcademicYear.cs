using ePTS.Entities.Gradebooks;
using ePTS.Entities.Reference;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Core
{
    // Represents a school academic year entity.
    [Table("SchoolAcademicYear")]
    [Comment("Represents a school academic year entity.")]
    public class SchoolAcademicYear : BaseEntity
    {
        public SchoolAcademicYear()
        {
            Gradebooks = new HashSet<Gradebook>();
        }

        // Unique identifier for each academic year record.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Academic Year")]
        [Comment("Unique identifier for each academic year record.")]
        [Column(Order = 1)]
        public Guid SchoolAcademicYearId { get; set; }

        // Unique identifier of the school that the academic year belongs to.
        [Display(Name = "School")]
        [Comment("Unique identifier of the school that the academic year belongs to. This is a foreign key that references the School table.")]
        [Column(Order = 2)]
        public Guid OrganizationId { get; set; }

        // Academic year reference.
        [Display(Name = "Academic Year", Prompt = "Select the academic year")]
        [Comment("A reference to the academic year, such as 2022, 2023, etc. This is a foreign key that references the RefAcademicYear table.")]
        [Column(Order = 3)]
        public int RefAcademicYearId { get; set; }

        // Registration date of the school academic year.
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [Comment("Date on which the school academic year was registered or added to the database.")]
        [DataType(DataType.Date)]
        [Column(Order = 4)]
        public DateTime RegistrationDate { get; set; }

        // Start date of the academic year.
        [Display(Name = "Start Date", Prompt = "Enter the start date")]
        [Comment("The starting date for the academic year.")]
        [DataType(DataType.Date)]
        [Column(Order = 5)]
        public DateTime? StartDate { get; set; }

        // End date of the academic year.
        [Display(Name = "End Date", Prompt = "Enter the end date")]
        [Comment("The ending date for the academic year.")]
        [DataType(DataType.Date)]
        [Column(Order = 6)]
        public DateTime? EndDate { get; set; }

        // Status of the academic year.
        [Display(Name = "Status")]
        [Comment("A reference to the status of the academic year, such as started, not started, completed, or closed. This is a foreign key that references the RefAcademicYearStatus table.")]
        [Column(Order = 7)]
        public int? RefAcademicYearStatusId { get; set; }

        // Indicates whether the academic year is missing enrollment data.
        [Display(Name = "Is Missing Enrollment")]
        [Column(Order = 8)]
        [Comment("Indicates whether the academic year is missing enrollment data.")]
        public bool? IsMissingEnrollment { get; set; } = null;

        // Navigation property referencing the School entity.
        [ForeignKey("OrganizationId")]
        [Display(Name = "Schools")]
        public virtual School? Schools { get; set; }

        // Navigation property referencing the RefAcademicYear entity.
        [ForeignKey("RefAcademicYearId")]
        [InverseProperty("SchoolAcademicYears")]
        [Display(Name = "Academic Years")]
        public virtual RefAcademicYear? AcademicYears { get; set; }

        // Navigation property referencing the RefAcademicYearStatus entity.
        [ForeignKey("RefAcademicYearStatusId")]
        [Display(Name = "Academic Year Status")]
        public virtual RefAcademicYearStatus? AcademicYearStatus { get; set; }

        // Collection navigation property representing the gradebooks associated with the academic year.
        public virtual ICollection<Gradebook> Gradebooks { get; set; }
    }
}

