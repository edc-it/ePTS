using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePTS.Models.ViewModels
{
    public class GradebookEnrollmentSummaryViewModel
    {
        public InfoPanelViewModel? InfoPanel { get; init; }
        public List<GradebookEnrollmentViewModel>? GradebookEnrollments { get; init; }
    }
}
