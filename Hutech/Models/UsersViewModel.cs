using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hutech.Models
{
    public class UsersViewModel
    {
        public List<UserViewModel> userViewModels { get; set; }
        public int? SelectedStatus { get; set; }
        public List<SelectListItem> Status { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? EmailId { get; set; }
        public string? EmployeeId { get; set; } 
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public long? SelectedDepartment { get; set; }
        public long? SelectedLocation { get; set; }
        public string? SelectedRole {  get; set; }
        public int? SelectedUserType { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Locations { get; set; }
        public List<SelectListItem> Roles { get; set; }
        public List<SelectListItem> UserTypes { get; set; }
    }
}
