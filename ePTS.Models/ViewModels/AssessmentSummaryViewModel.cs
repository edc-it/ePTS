using ePTS.Entities.Assessments;
using ePTS.Entities.Gradebooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePTS.Models.ViewModels
{
    public class AssessmentSummaryViewModel
    {
        public InfoPanelViewModel? InfoPanel { get; init; }
        public List<AssessmentResultsViewModel>? AssessmentResults { get; init; }
        public List<AssessmentPerformanceLevelsViewModel>? AssessmentPerformanceLevels { get; init; }
    }
}
