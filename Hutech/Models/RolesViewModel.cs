using System.ComponentModel.DataAnnotations;

namespace Hutech.Models
{
    public class RolesViewModel
    {
        public List<RoleViewModel> Roles { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public string? RoleName { get; set; }
        public string? UpdatedBy { get; set; }
        [Display(Name = "Date")]
        // [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? UpdatedDate { get; set; }
    }
}
