using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ePTS.Models.ViewModels
{
    public class OrganizationParentViewModel
    {
        [Display(Name = "Index")]
        public int? Index { get; set; }

        [Display(Name = "Organization")]
        public Guid? OrganizationId { get; set; }

        [Display(Name = "Organization")]
        public string? OrganizationName { get; set; }
        public string? OrganizationType { get; set; }

        [Display(Name = "Parent Organization")]
        public Guid? ParentOrganizationId { get; set; }

        [Display(Name = "Parent")]
        public string? Parent { get; set; }

        [Display(Name = "Is OrganizationUnit?")]
        public bool? IsOrganizationUnit { get; set; }
    }
    
}
