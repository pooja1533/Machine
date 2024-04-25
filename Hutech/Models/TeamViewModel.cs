using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hutech.Models
{
    public class TeamViewModel
    {
        public TeamViewModel()
        {
            locations = new List<SelectListItem>();
            deparments = new List<SelectListItem>();
        }
        public long Id { get; set; }
        public string? Name { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public long LocationId { get; set; }
        public long DepartmentId { get; set; }  
        public List<SelectListItem> locations { get; set; }
        public List<SelectListItem> deparments { get; set; }
        public string? LocationName { get; set; }
        public string? DepartmentName { get; set; }
        public DateTime? DatecreatedUtc { get; set; }
        public string? CreatedByUserId { get; set; }
        public DateTime DateModifiedUtc { get; set; }
        public string? ModifiedByUserId { get; set; }
        [NotMapped]
        public string? fullname { get; set; }
        [NotMapped]
        public string? Role { get; set; }
    }
    public class TeamValidator : AbstractValidator<TeamViewModel>
    {
        public TeamValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter Team Name");
            RuleFor(x => x.LocationId).NotNull().NotEmpty().WithMessage("Please select Location");
            RuleFor(x => x.DepartmentId).NotNull().NotEmpty().WithMessage("Please select Department");
        }
    }
    public class TeamModel
    {
        public string? TeamName { get; set; }
        public string? UpdatedBy { get; set; }
        public int PageNumber { get; set; }
        public string? Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? LocationName { get; set; }
        public string? DepartmentName { get; set; }
    }
}
