using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string DepartmentName { get; set; }
        public string Remark { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime PerformedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifyByUserId { get; set; }
        public DateTime? ModifiedDate { get; set;}
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public string? DocumentId { get; set; }
        [NotMapped]
        public string? Path { get; set; }
        [NotMapped]
        public string? InstrumentIdName { get; set; }
        [NotMapped]
        public string? fullname { get; set; }
        [NotMapped]
        public string? Role { get; set; }
    }
}
