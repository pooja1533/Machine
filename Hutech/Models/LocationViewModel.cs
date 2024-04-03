using FluentValidation;

namespace Hutech.Models
{
    public class LocationViewModel
    {
        public string Name { get; set; }
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedByUserId { get; set; }
    }
    public class LocationValidator : AbstractValidator<LocationViewModel>
    {
        public LocationValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter Location Name");
        }
    }
}
