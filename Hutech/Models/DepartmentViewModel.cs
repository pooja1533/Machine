using FluentValidation;

namespace Hutech.Models
{
    public class DepartmentViewModel
    {
        public string Name { get; set; }
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class DepartmentValidator : AbstractValidator<DepartmentViewModel>
    {
        public DepartmentValidator() 
        { 
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Please enter Department Name")
            .Matches(@"^[A-Za-z\s]*$").WithMessage("Department Name should only contain letters.");
        }
    }
}
