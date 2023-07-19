using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ePTS.Models.ViewModels
{
    public class LocationParentViewModel
    {
        [Display(Name = "Index")]
        public int? Index { get; set; }

        [Display(Name = "Location")]
        public string? RefLocationId { get; set; }

        [Display(Name = "Location")]
        public string? LocationName { get; set; }

        [Display(Name = "Parent Location")]
        public string? ParentLocationId { get; set; }

        [Display(Name = "Parent")]
        public string? ParentName { get; set; }

        [Display(Name = "Location Type")]
        public int? RefLocationTypeId { get; set; }

        [Display(Name = "Location Type")]
        public string? LocationType { get; set; }

        [Display(Name = "Location Level")]
        public int? LocationLevel { get; set; }
    }
}
