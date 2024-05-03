using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public class Audit
    {
        public long AuditId { get; set; }
        public string ModuleName { get; set; }
        public string UserId { get; set; }
        public string Request_Data { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public string IPAddress { get; set; }
        public string role { get; set; }

    }
}
