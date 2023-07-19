using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePTS.Entities.Audit
{
    public class ApplicationLog
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? MessageTemplate { get; set; }
        public string? Level { get; set; }
        public DateTimeOffset? TimeStamp { get; set; }
        public string? Exception { get; set; }
        public string? JsonLog { get; set; }
        public string? Properties { get; set; }
        public string? LogEvent { get; set; }
    }
}
