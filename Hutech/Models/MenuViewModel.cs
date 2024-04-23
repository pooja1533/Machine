using System.ComponentModel.DataAnnotations.Schema;

namespace Hutech.Models
{
    public class MenuViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }
        public bool Isdeleted { get; set; } 
        public string? URL { get; set; }
        public int sort { get; set; }
        public string ParentName { get; set; }
        [NotMapped]
        public bool? IsUserHaveAccess { get; set; }
    }
}
