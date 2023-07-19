using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePTS.Models.ViewModels
{
    public class GradebookAssessmentAverageViewModel
    {
        public Guid GradebookId { get; set; }
        public double? AssessedFemale { get; set; }
        public double? AssessedMale { get; set; }
        public double? Assessed { get; set; }
    }
}
