using ePTS.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePTS.Models.ViewModels
{
    public class InfoPanelViewModel
    {
        public int? CurrentLevel { get; set; }
        public string? CurrentController { get; set; }

        //

        public Guid? OrganizationId { get; set; }
        public Guid? SchoolAcademicYearId { get; set; }
        public Guid? GradebookId { get; set; }
        public Guid? GradebookAssessmentId { get; init; }
        
        [Display(Name = "Organization Name")]
        public string? OrganizationName { get; init; }

        [Display(Name = "Organization Type")]
        public string? OrganizationType { get; init; }
        
        [Display(Name = "Organization Type")]
        public int? OrganizationTypeId { get; init; }
        
        [Display(Name = "Is School?")]
        public bool? IsSchool { get; init; }

        [Display(Name = "Code")]
        public string? Code { get; init; }
        
        [Display(Name = "Zone")]
        public string? Zone { get; init; }

        [Display(Name = "Address")]
        public string? Address { get; init; }
        
        [Display(Name = "Latitude")]
        public double? Latitude { get; init; }
        
        [Display(Name = "Longitude")]
        public double? Longitude { get; init; }

        [Display(Name = "EMIS #")]
        public string? EMIS { get; init; }

        [Display(Name = "School Type")]
        public string? SchoolType { get; init; } = string.Empty;

        [Display(Name = "Location")]
        public string? SchoolLocation { get; init; }

        [Display(Name = "Administration Type")]
        public string? SchoolAdministrationType { get; init; }

        [Display(Name = "Language of Instruction")]
        public string? SchoolLanguage { get; init; }

        [Display(Name = "Academic Year")]
        public string? SchoolAcademicYear { get; init; }

        [Display(Name = "Grade Level")]
        public string? GradeLevel { get; init; }

        [Display(Name = "Term")]
        public string? AssessmentTerm { get; init; }

        [Display(Name = "Week")]
        public string? AssessmentWeek { get; init; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Registration Date")]
        public DateTime? RegistrationDate { get; init; }

        [Display(Name = "Assessed Female")]
        public int? AssessedFemale { get; init; }

        [Display(Name = "Assessed Male")]
        public int? AssessedMale { get; init; }

        [Display(Name = "Assessed")]
        public int? Assessed { get; init; }

        [Display(Name = "Enrolled Female")]
        public int? EnrolledFemale { get; init; }

        [Display(Name = "Enrolled Male")]
        public int? EnrolledMale { get; init; }

        [Display(Name = "Enrolled")]
        public int? Enrolled { get; init; }

        public bool? IsOrganizationUnit { get; set; }
        public Guid? UserOrganizationId { get; set; }

        public IEnumerable<Organization>? OrganizationParent { get; set; }
        public IEnumerable<LocationParentViewModel>? LocationParent { get; init; }
        public string? RefLocationId { get; set; }
        public string? AcademicYear { get; set; }
        public Guid? ParentOrganizationId { get; set; }
        public bool? IsMissingAssessmentResults { get; set; }
    }
}
