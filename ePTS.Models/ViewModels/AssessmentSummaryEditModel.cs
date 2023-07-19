using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ePTS.Entities.Assessments;

namespace ePTS.Models.ViewModels
{
    public class AssessmentSummaryEditModel
    {
        [Required(ErrorMessage = "The Data Entry Date is required")]
        [Display(Name = "Data Entry Date", Prompt = "Select the data entry date")]
        [DataType(DataType.Date)]
        public DateTime? RegistrationDate { get; set; }
        public InfoPanelViewModel? InfoPanel { get; init; }
        public List<AssessmentResultsEditModel>? AssessmentResults { get; set; }
        public List<AssessmentPerformanceLevelsEditModel>? AssessmentPerformanceLevels { get; set; }

    }
}
