using FluentValidation;

namespace Hutech.Models
{
    public class RequirementViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class RequirementValidator:AbstractValidator<RequirementViewModel>
    {
        public RequirementValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter Requirement Name")
            .Matches(@"^[A-Za-z\s]*$").WithMessage("Requirement Name should only contain letters.");
        }
    }
}
