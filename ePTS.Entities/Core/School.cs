using ePTS.Entities.Reference;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Core
{
    // The School class represents a school entity and inherits from the Organization base class.
    [Table("School")]
    public class School : Organization, IAuditable
    {
        public School()
        {
            // Initialize the collections in the constructor.
            SchoolAcademicYears = new HashSet<SchoolAcademicYear>();
        }

        // Represents the unique code of the school.
        [Display(Name = "School Code", Prompt = "Enter the school code")]
        [MaxLength(100)]
        [Comment("A short code that represents the unique school (such as the school EMIS number)")]
        [Column(Order = 12)]
        public string? SchoolCode { get; set; }

        // Represents the type of the school.
        [Display(Name = "Type", Prompt = "Select the school type")]
        [Comment("A reference to the type of school, such as primary, secondary, or vocational. This is a foreign key that references the SchoolType table")]
        [Column(Order = 13)]
        public int? RefSchoolTypeId { get; set; }

        // Represents the location of the school.
        [Display(Name = "Location", Prompt = "Select the school location")]
        [Comment("A reference to the location of the school, such as rural or urban. This is a foreign key that references the SchoolLocation table")]
        [Column(Order = 14)]
        public int? RefSchoolLocationId { get; set; }

        // Represents the administration type of the school.
        [Display(Name = "Administration Type", Prompt = "Select the school administration type")]
        [Comment("A reference to the type of administration for the school, such as private or public. This is a foreign key that references the SchoolAdministrationType table")]
        [Column(Order = 15)]
        public int? RefSchoolAdministrationTypeId { get; set; }

        // Represents the language of instruction for the school.
        [Display(Name = "Language", Prompt = "Select the school language")]
        [Comment("A reference to the language of instruction for the school. This is a foreign key that references the SchoolLanguage table")]
        [Column(Order = 16)]
        public int? RefSchoolLanguageId { get; set; }

        // Represents the status of the school.
        [Display(Name = "Status", Prompt = "Select the school status")]
        [Comment("A reference to the status of the school, such as active or inactive. This is a foreign key that references the SchoolStatus table")]
        [Column(Order = 17)]
        public int? RefSchoolStatusId { get; set; }

        // Represents the head teacher or principal of the school.
        [Display(Name = "Head Teacher", Prompt = "Enter the head teacher's name")]
        [MaxLength(150)]
        [Comment("The name of the head teacher or principal of the school")]
        [Column(Order = 18)]
        public string? HeadTeacher { get; set; }

        // Represents the opening date of the school.
        [Display(Name = "Opening Date", Prompt = "Enter the school's opening date")]
        [Comment("The date when the school was opened")]
        [DataType(DataType.Date)]
        [Column(Order = 19)]
        public DateTime? OpeningDate { get; set; }

        // Represents the closing date of the school.
        [Display(Name = "Closing Date", Prompt = "Enter the school's closing date")]
        [Comment("The date when the school was closed")]
        [DataType(DataType.Date)]
        [Column(Order = 20)]
        public DateTime? ClosingDate { get; set; }

        // Navigation property referencing the SchoolType entity.
        [ForeignKey("RefSchoolTypeId")]
        [Display(Name = "School Type")]
        public virtual RefSchoolType? SchoolTypes { get; set; }

        // Navigation property referencing the SchoolLocation entity.
        [ForeignKey("RefSchoolLocationId")]
        [Display(Name = "School Location")]
        public virtual RefSchoolLocation? SchoolLocations { get; set; }

        // Navigation property referencing the SchoolLanguage entity.
        [ForeignKey("RefSchoolLanguageId")]
        [Display(Name = "School Language")]
        public virtual RefSchoolLanguage? SchoolLanguages { get; set; }

        // Navigation property referencing the SchoolAdministrationType entity.
        [ForeignKey("RefSchoolAdministrationTypeId")]
        [Display(Name = "School Administration Type")]
        public virtual RefSchoolAdministrationType? SchoolAdministrationTypes { get; set; }

        // Navigation property referencing the SchoolStatus entity.
        [ForeignKey("RefSchoolStatusId")]
        [Display(Name = "Status")]
        public virtual RefSchoolStatus? SchoolStatus { get; set; }

        // Collection navigation property representing the academic years associated with the school.
        public virtual ICollection<SchoolAcademicYear> SchoolAcademicYears { get; set; }

    }
}
