using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hutech.Models
{
    public class UsersViewModel
    {
        public List<UserViewModel> userViewModels { get; set; }
        public int SelectedStatus { get; set; }
        public List<SelectListItem> Status { get; set; }
    }
}
