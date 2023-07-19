using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePTS.Models.ViewModels
{
    public class GradebookEnrollmentViewModel
    {
        public Guid? GradebookEnrollmentId { get; set; }
        public Guid? GradebookId { get; set; }
        public string? ParticipantType { get; set; }
        public string? Gradebook { get; set; }
        public int? Male { get; set; }
        public int? Female { get; set; }
        public int? Total { get; set; }
    }
}
