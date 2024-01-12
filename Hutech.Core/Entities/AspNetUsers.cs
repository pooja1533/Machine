using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public class AspNetUsers
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }

        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        [NotMapped]
        public string FirstName { get; set; }
        [NotMapped]
        public string LastName { get; set; }
        [NotMapped]
        public string PhoneNumber { get; set; }
        [NotMapped]
        public string Address { get; set; }
        [NotMapped]
        public string RoleId { get; set; }
        [NotMapped]
        public string RoleName { get; set; }
        [NotMapped]
        public string UserRole { get; set; }
    }
   
    
}
