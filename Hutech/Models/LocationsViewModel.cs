using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Hutech.Models
{
    public class LocationsViewModel
    {
        public List<LocationViewModel> locationViewModel {  get; set; }
        public string? LocationName { get; set; }
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
