using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hutech.Models
{
    public class InstrumentActivitiesViewModel
    {
        public List<InstrumentActivityViewModel> instrumentActivityViewModels { get; set; }
        public string? InstrumentActivityId { get; set; }
        public string? ActivityName { get; set; }
        public string? InstrumentName { get; set; }
        public string? RequirementName { get; set; }
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
    }
}
