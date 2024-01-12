using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hutech.Models
{
    public class InstrumentActivityViewModel
    {
        public InstrumentActivityViewModel()
        {
            Instruments = new List<SelectListItem>();
            Activities= new List<SelectListItem>();
            Requirement= new List<SelectListItem>();
            Department= new List<SelectListItem>();
        }
        public List<SelectListItem> Instruments { get; set; }
        public List<SelectListItem> Activities { get; set; }
        public List<SelectListItem> Requirement { get; set; }
        public List<SelectListItem> Department { get; set; }
        public long InstrumentId { get; set; }
        public long ActivityId { get; set; }
        public long RequirementId { get; set; }
        public long DepartmentId { get; set; }
        public string InstrumentActivity { get; set; }
        public bool Frequency { get; set; }
        public string FrequencyYear { get; set; }
        public string FrequencyMonth { get; set; }
        public string FrequencyDay { get; set;}
        public long Days { get; set; }
        public string BeforeAlerts { get; set; }
    }
}
