using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public class UserDetail:IdentityUser
    {
        public Guid Id { get; set; }
        public string AspNetUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string EmployeeId { get; set; }
        public string? Remark { get; set; }
        public int LoginValidityDays { get; set; }
        public long UserTypeId { get; set; }
        public long LocationId { get; set; }
        public long DepartmentId { get; set; }
        public long UserRoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
        public string? WindowsUserName { get; set; }
        public List<string> SelectedUserRoleId { get; set; }
        [NotMapped]
        public string? FullName { get; set; }
        [NotMapped]
        public string? DepartmentName { get; set; }
        [NotMapped]
        public string? LocationName { get; set; }
        [NotMapped]
        public string? RoleName { get; set; }
        [NotMapped]
        public string? UserType { get; set; }
        public int? UserstatusId { get; set; }
        [NotMapped]
        public long UserId { get; set; }
        [NotMapped]
        public string? UserstatusName { get; set; }
        public string? RemarkForReject { get; set; }

    }
}
