using ePTS.Entities.Enrollments;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ePTS.Entities.Core
{
    [Table("School")]
    public class School : Organization
    {
        public School()
        {
            SchoolAcademicYears = new HashSet<SchoolAcademicYear>();
            SchoolEnrollments = new HashSet<SchoolEnrollment>();
        }

        [Display(Name = "School Code", Prompt = "Enter the school code")]
        [MaxLength(100)]
        [Comment("A short code that represents the unique school (such as the school EMIS number)")]
        [Column(Order = 1)]
        public string? SchoolCode { get; set; }

        [Display(Name = "School Type", Prompt = "Select the school type")]
        [Comment("A reference to the type of school, such as primary, secondary, or vocational. This is a foreign key that references the SchoolType table")]
        [Column(Order = 2)]
        public int? RefSchoolTypeId { get; set; }

        [Display(Name = "School Location", Prompt = "Select the school location")]
        [Comment("A reference to the location of the school, such as rural or urban. This is a foreign key that references the SchoolLocation table")]
        [Column(Order = 3)]
        public int? RefSchoolLocationId { get; set; }

        [Display(Name = "School Administration Type", Prompt = "Select the school administration type")]
        [Comment("A reference to the type of administration for the school, such as private or public. This is a foreign key that references the SchoolAdministrationType table")]
        [Column(Order = 4)]
        public int? RefSchoolAdministrationTypeId { get; set; }

        [Display(Name = "School Language", Prompt = "Select the school language")]
        [Comment("A reference to the language of instruction for the school. This is a foreign key that references the SchoolLanguage table")]
        [Column(Order = 5)]
        public int? RefSchoolLanguageId { get; set; }

        [Display(Name = "School Status", Prompt = "Select the school status")]
        [Comment("A reference to the status of the school, such as active or inactive. This is a foreign key that references the SchoolStatus table")]
        [Column(Order = 6)]
        public int? RefSchoolStatusId { get; set; }

        [Display(Name = "Head Teacher", Prompt = "Enter the head teacher''s name")]
        [MaxLength(150)]
        [Comment("The name of the head teacher or principal of the school")]
        [Column(Order = 7)]
        public string? HeadTeacher { get; set; }

        [Display(Name = "Opening Date", Prompt = "Enter the school''s opening date")]
        [Comment("The date when the school was opened")]
        [Column(Order = 8)]
        public DateTime OpeningDate { get; set; }

        [Display(Name = "Closing Date", Prompt = "Enter the school''s closing date")]
        [Comment("The date when the school was closed")]
        [Column(Order = 9)]
        public DateTime ClosingDate { get; set; }

        public virtual ICollection<SchoolAcademicYear> SchoolAcademicYears { get; set; }
        public virtual ICollection<SchoolEnrollment> SchoolEnrollments { get; set; }
    }
}

