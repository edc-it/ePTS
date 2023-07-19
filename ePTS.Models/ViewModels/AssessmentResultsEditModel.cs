using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ePTS.Models.ViewModels
{
    public class AssessmentResultsEditModel
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Assessment Result")]
        public Guid AssessmentResultId { get; set; }

        [Display(Name = "Gradebook Assessment", Prompt = "Select the gradebook assessment name")]
        public Guid GradebookAssessmentId { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Registration Date", Prompt = "Enter the registration date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Assessment Item", Prompt = "Select the assessment item")]
        public Guid AssessmentItemId { get; set; }

        [Display(Name = "Total Value", Prompt = "Enter the total number of correct responses from all participants")]
        [Required(ErrorMessage = "The {0} field is required.")]
        public int? Score { get; set; }

        public string? AssessmentTerm { get; init; }
        public string? AssessmentWeek { get; init; }
        public string? AssessmentItemText { get; init; }
        public string? AssessmentCategory { get; init; }
        public int? SortOrder { get; init; }

    }
}
