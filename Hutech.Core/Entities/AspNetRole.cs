using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public class AspNetRole : IdentityRole<Guid>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
        public DateTime DatecreatedUtc { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
        public string? ModifiedByUserId { get; set; }
        [NotMapped]
        public string? fullname { get; set; }
        [NotMapped]
        public string? Role {  get; set; }
    }
}
