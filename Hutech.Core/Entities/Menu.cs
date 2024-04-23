using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public class Menu
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }
        public bool Isdeleted { get; set; }
        public string? URL { get; set; }
        public int sort { get; set; }
        [NotMapped]
        public string? ParentName { get; set; }
        [NotMapped]
        public bool? IsUserHaveAccess { get; set; }
    }
}
