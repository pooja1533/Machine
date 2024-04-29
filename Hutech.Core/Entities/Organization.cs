using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public  class Organization
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string? ModifiedByUserId { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
    }
}
