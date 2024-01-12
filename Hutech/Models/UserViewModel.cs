using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hutech.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string LastName { get; set; }
        public Guid? RoleId { get; set; }
        public List<SelectListItem> Roles { get; set; }
        [NotMapped]
        public string? UserRole { get; set; }
    }
}
