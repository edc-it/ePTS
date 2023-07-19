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
    public class GradebookViewModel
    {
        public Guid GradebookId { get; set; }
        public Guid SchoolAcademicYearId { get; set; }

        
        [Display(Name = "Grade Level")]
        public int RefGradeLevelId { get; set; }
        [Display(Name = "Grade Level")]
        public string? GradeLevel { get; set; }

        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }
        public Guid? GradebookPeriodId { get; set; }
        public Guid? AssessmentId { get; set; }

        public int? RefAssessmentPlatformTypeId { get; set; }
        public int? RefGradebookStatusId { get; set; }
        public bool? IsMissingGradebookAssessments { get; set; } = null!;

        public int? GradebookAssessmentsCount { get; set; }
        public string? Platform { get; set; }
        public int? SortOrder { get; set; }
    }
}
