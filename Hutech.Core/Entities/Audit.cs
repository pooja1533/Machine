using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public class Audit
    {
        public long AuditId { get; set; }
        public string ControllerName { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public DateTime CurrentDatetime { get; set; }

    }
}
