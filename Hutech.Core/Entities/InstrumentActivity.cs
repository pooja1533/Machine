using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Hutech.Core.Entities
{
    public class InstrumentActivity
    {
        public InstrumentActivity() {
            Instruments = new List<SelectListItem>();
            Activities = new List<SelectListItem>();
            Requirement = new List<SelectListItem>();
            Department = new List<SelectListItem>();
            //EmailList = new List<SelectListItem>();

        }
        public long Id { get; set; }
        public List<SelectListItem> Instruments { get; set; }
        public List<SelectListItem> Activities { get; set; }
        public List<SelectListItem> Requirement { get; set; }
        public List<SelectListItem> Department { get; set; }
        public string? InstrumentActivityId { get; set; }
        public long InstrumentId { get; set; }
        public long ActivityId { get; set; }
        public long RequirementId { get; set; }
        public long DepartmentId { get; set; }
        public string InstrumentActivityName { get; set; }
        public int Days { get; set; }
        public bool BeforeAlerts { get; set; }
        public List<SelectListItem>? EmailList { get; set; }
        public List<string> SelectedEmailListInt { get; set; }
        public string Frequency { get; set; }
        public int FrequencyTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string MyEumJson
        {
            get
            {
                return JsonConvert.SerializeObject(Enum.GetValues(typeof(FrequencyEnum)), new Newtonsoft.Json.Converters.StringEnumConverter());
            }
            set { }
        }
        public string? InstrumentName { get; set; }
        public string? activityName { get; set; }
        public string? RequirementName { get; set; }
        public string? DeaprtmentName { get; set; }
        public int? BeforeAlertsTime { get; set; }
        [NotMapped]
        public string? SelectedGroups { get; set; }

        [NotMapped]
        public string? InstrumentActivityGroup { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string CreatedByUserId { get; set; }
        public string? ModifiedByUserId { get; set; }
    }
    public enum FrequencyEnum
    {
        FrequencyDay = 1,
        FrequencyMonth = 2,
        FrequencyYear = 3,
    }
}
