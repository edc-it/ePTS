using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePTS.Models.ViewModels
{
    public class GradebookSummaryViewModel
    {
        public InfoPanelViewModel? InfoPanel { get; init; }
        public List<GradebookViewModel>? Gradebooks { get; init; }
    }
}
