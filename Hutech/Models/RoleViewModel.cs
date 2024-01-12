using FluentValidation;

namespace Hutech.Models
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }
       public string Name { get; set; }
    }
    public class RoleValidator : AbstractValidator<RoleViewModel>
    {
        public RoleValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please Enter Role Name")
            .Matches(@"^[A-Za-z\s]*$").WithMessage("RoleName should only contain letters.");
        }
    }
}
