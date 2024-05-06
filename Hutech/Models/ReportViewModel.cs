
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hutech.Models
{
    public class ReportViewModel
    {
        public List<SelectListItem> Reports { get; set; }
        public string ReportId { get; set; }
    }
}
