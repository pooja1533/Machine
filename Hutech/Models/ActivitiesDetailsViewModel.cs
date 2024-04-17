using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hutech.Models
{
    public class ActivitiesDetailsViewModel
    {
        public List<ActivityDetailsViewModel> activityDetailsViewModels { get; set; }
        public string? InstrumentIdName { get; set; }
        public string? InstrumentName { get; set; }
        public string? InstrumentSerial { get; set; }
        public string? Model { get; set; }
        public string? LocationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? UpdatedBy { get; set; }
        public int SelectedStatus { get; set; }
        public List<SelectListItem> Status { get; set; }
        [Display(Name = "Date")]
        // [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? UpdatedDate { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        [Display(Name = "Date")]
        // [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PerformedDate { get; set; }
    }
}
