using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public class MachineComment
    {
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
        public long MachineId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
