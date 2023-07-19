using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePTS.Models.ViewModels
{
    public class OrganizationSelectViewModel
    {
        public Guid OrganizationId { get; set; }
        public string? OrganizationName { get; set; }
        public string? Code { get; set; }
        public string? OrganizationType { get; set; }

        public bool? IsOrganizationUnit { get; set; }
    }
}
