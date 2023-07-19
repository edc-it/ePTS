using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePTS.Models.ViewModels
{
    public record AssessmentResultsViewModel
    {
        public Guid? AssessmentResultId { get; init; }
        public string? GradeLevel { get; set; }
        public string? AssessmentTerm { get; init; }
        public string? AssessmentWeek { get; init; }
        public string? AssessmentItemText { get; init; }
        public string? AssessmentCategory { get; init; }
        public int? Score { get; init; }
        public int? SortOrder { get; init; }

        public int? Responses { get; set; }

    }
}
