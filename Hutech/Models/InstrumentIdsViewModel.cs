using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hutech.Models
{
    public class InstrumentIdsViewModel
    {
        public List<InstrumentIdViewModel> instrumentIdViewModels { get; set; }
        public string? InstrumentIdName { get; set; }
        public string? Model {  get; set; }
        public string? InstrumentName { get; set; }
        public string? InstrumentSerial { get; set; }
        public string? InstrumentLocation { get; set; }
        public string? TeamName { get; set; }   
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
