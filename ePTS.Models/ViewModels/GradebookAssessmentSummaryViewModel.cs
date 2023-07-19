using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePTS.Models.ViewModels
{
    public class GradebookAssessmentSummaryViewModel
    {
        public InfoPanelViewModel? InfoPanel { get; init; }
        public List<GradebookAssessmentViewModel>? GradebookAssessments { get; init; }
    }
}
