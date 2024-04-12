using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public class InstrumentsIds
    {
        public InstrumentsIds()
        {

        }
        public long Id { get; set; }
        public string InstrumentsId { get; set; }
        public string Model { get; set; }
        public string InstrumentSerial { get; set; }
        public long InstrumentId { get; set; }
        public long LocationId { get; set; }
        public long TeamId { get; set; }
        public string TeamLocation { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public string? LocationName { get; set; }
        [NotMapped]
        public string? InstrumentName { get; set; }
        [NotMapped]
        public string? TeamName { get; set;}
        public string CreatedByUserId { get; set; }
        public string? ModifiedByUserId { get; set; }
        public DateTime DatecreatedUtc { get; set; }
        public DateTime DateModifiedUtc { get; set; }
        [NotMapped]
        public string? FullName { get; set; }
        [NotMapped]
        public string? Role { get; set; }
    }
}
