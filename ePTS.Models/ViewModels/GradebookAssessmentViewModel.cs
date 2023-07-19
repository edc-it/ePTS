using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ePTS.Models.ViewModels
{
    public class GradebookAssessmentViewModel
    {
        //public Guid? GradebookAssessmentId { get; init; }
        //public string? OrganizationName { get; init; }
        //public string? OrganizationType { get; init; }
        //public string? Code { get; init; }
        //public string? Province { get; init; }
        //public string? District { get; init; }
        //public string? Zone { get; init; }
        //public string? Address { get; set; }
        //public double? Latitude { get; init; }
        //public double? Longitude { get; init; }
        //public string? EMIS { get; init; }
        //public string? SchoolType { get; init; }
        //public string? SchoolLocation { get; init; }
        //public string? SchoolAdministrationType { get; init; }
        //public string? SchoolLanguage { get; init; }
        //public string? SchoolAcademicYear { get; init; }
        //public string? GradeLevel { get; init; }
        //public string? AssessmentTerm { get; init; }
        //public string? AssessmentWeek { get; init; }

        public Guid GradebookAssessmentId { get; set; }
        public Guid GradebookId { get; set; }
        public Guid GradebookAssessmentPeriodId { get; set; }

        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Assessed Female")]
        public int? AssessedFemale { get; set; }

        [Display(Name = "Assessed Male")]
        public int? AssessedMale { get; set; }
        public int? Assessed { get; set; }

        [Display(Name = "Gradebook Assessment Status")]
        public int? GradebookAssessmentStatusId { get; set; }
        public string? GradebookAssessmentStatus { get; set; }

        [Display(Name = "IsMissingAssessmentResults")]
        public bool? IsMissingAssessmentResults { get; set; }

        [Display(Name = "Grade Level")]
        public string? GradeLevel { get; init; }

        [Display(Name = "Term")]
        public string? AssessmentTerm { get; init; }

        [Display(Name = "Week")]
        public string? AssessmentWeek { get; init; }

        public int? SortOrder { get; set; }
    }
}
