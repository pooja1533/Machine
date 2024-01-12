using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hutech.Models
{
    public class TeamViewModel
    {
        public TeamViewModel()
        {
            locations = new List<SelectListItem>();
        }
        public long Id { get; set; }
        public string Name { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public long LocationId { get; set; }
        public List<SelectListItem> locations { get; set; }
        public string? LocationName { get; set; }
    }
    public class TeamValidator : AbstractValidator<TeamViewModel>
    {
        public TeamValidator() 
        { 
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Please enter Name")
            .Matches(@"^[A-Za-z\s]*$").WithMessage("Team Name should only contain letters.");
        }
    }
}
