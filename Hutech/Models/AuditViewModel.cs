using System.ComponentModel.DataAnnotations;

namespace Hutech.Models
{
    public class AuditViewModel
    {
        public long AuditId { get; set; }
        public string ControllerName { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public DateTime CurrentDatetime { get; set; }
    }
}
