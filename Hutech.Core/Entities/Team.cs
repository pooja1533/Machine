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

        [NotMapped]
        public string LocationName { get; set; }
    }
}
