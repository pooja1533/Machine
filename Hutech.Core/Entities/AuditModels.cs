using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public class AuditModels
    {
        public int AuditId { get; set; }
        public string Area { get; set; }
        public string ModuleName { get; set; }
        public string ActionName { get; set; }
        public string RoleId { get; set; }
        public string LangId { get; set; }
        public string IpAddress { get; set; }
        public string IsFirstLogin { get; set; }
        public string LoggedInAt { get; set; }
        public string LoggedOutAt { get; set; }
        public string Request_Data { get; set; }
        public string PageAccessed { get; set; }
        public string SessionId { get; set; }
        public string UrlReferrer { get; set; }
        public string UserId { get; set; }
        public string? Exception_Details { get; set; }
    }
}
