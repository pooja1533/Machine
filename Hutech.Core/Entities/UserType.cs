using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public  class UserType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string? ModifiedByUserId { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
