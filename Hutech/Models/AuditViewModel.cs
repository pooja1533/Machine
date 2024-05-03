using System.ComponentModel.DataAnnotations;

namespace Hutech.Models
{
    public class AuditViewModel
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
