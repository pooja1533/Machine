using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public class Team
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long LocationId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DatecreatedUtc { get; set; }
        public string? CreatedByUserId { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
        public string? ModifiedByUserId { get; set; }

        [NotMapped]
        public string LocationName { get; set; }
        [NotMapped]
        public string? fullname { get; set; }
        [NotMapped]
        public string? Role { get; set; }
    }
}
