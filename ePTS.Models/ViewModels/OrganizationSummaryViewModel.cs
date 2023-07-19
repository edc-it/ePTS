using ePTS.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePTS.Models.ViewModels
{
    public class OrganizationSummaryViewModel
    {
        public InfoPanelViewModel? InfoPanel { get; init; }
        public List<Organization>? Organizations { get; init; }
        public List<Organization>? Breadcrumb { get; set; }
    }
}
