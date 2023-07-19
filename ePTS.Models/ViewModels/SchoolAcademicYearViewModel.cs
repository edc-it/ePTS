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
    public class SchoolAcademicYearViewModel
    {
        public Guid SchoolAcademicYearId { get; set; }
        public Guid OrganizationId { get; set; }
        public int RefAcademicYearId { get; set; }

        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Status")]
        public int? RefAcademicYearStatusId { get; set; }

        [Display(Name = "Is Missing Enrollment")]
        public bool? IsMissingEnrollment { get; set; } = null;

        [Display(Name = "Academic Year")]
        public string? AcademicYear { get; set; }

        [Display(Name = "Academic Year Status")]
        public string? AcademicYearStatus { get; set; }

        public int? SortOrder { get; set; }

        public int? GradebookCount { get; set; }
        public string? OrganizationType { get; set; }
        public int? OrganizationTypeId { get; set; }
        public string? Code { get; set; }
        public string? Zone { get; set; }
    }
}
