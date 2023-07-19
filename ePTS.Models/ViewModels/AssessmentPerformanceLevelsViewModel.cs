using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ePTS.Models.ViewModels
{
    public class AssessmentPerformanceLevelsViewModel
    {
        [Display(Name = "Gradebook Assessment", Prompt = "Select the gradebook assessment name")]
        public Guid GradebookAssessmentId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Performance Level")]
        public Guid AssessmentPerformanceLevelId { get; set; }

        [Display(Name = "Performance Level")]
        public int RefPerformanceLevelId { get; set; }

        [Display(Name = "Sex", Prompt = "Select the sex")]
        public int RefSexId { get; set; }

        [Display(Name = "Score Value", Prompt = "Enter the score value")]
        public int? Score { get; set; }

        public string? GradeLevel { get; set; }
        public string? AssessmentTerm { get; set; }
        public string? AssessmentWeek { get; set; }
        public string? PerformanceLevel { get; set; }
        public string? Code { get; set; }
        public string? Sex { get; set; }
        public string? SexId { get; set; }
        public string? Color { get; set; }
        public int? Assessed { get; set; }
        public int? PerformanceLevelSortOrder { get; set; }
        public int? SexSortOrder { get; set; }
        public double? Percent { get; set; }

    }
}
