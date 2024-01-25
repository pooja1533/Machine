using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Core.Entities
{
    public class ActivityDetails
    {
        public long Id { get; set; }
        public long InstrumentId { get; set; }
        public string InstrumentName { get; set; }
        public string InstrumentSerial { get; set; }
        public string Model { get; set; }
        public string LocationName { get; set; }
        public long InstrumentActivityId { get; set; }
        public int Days { get; set; }
        public string Frequency { get; set; }
        public string TeamName { get; set; }
        public string TeamLocation { get; set; }
        public string RequirementName { get; set; }
        public string DeaprtmentName { get; set; }
        public string Remark { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime PerformedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifyByUserId { get; set; }
        public DateTime? ModifiedDate { get; set;}
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
