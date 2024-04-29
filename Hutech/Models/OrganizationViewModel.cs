using FluentValidation;

namespace Hutech.Models
{
    public class OrganizationViewModel
    {
        public int Id { get; set; }    
        public string? Name { get; set; }
        public bool IsActive { get; set; }      
        public bool IsDeleted { get; set; } 
        public string CreatedByUserId { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string? ModifiedByUserId { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
    }
    public class OrganizationValidator:AbstractValidator<OrganizationViewModel>
    {
        public OrganizationValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please Enter Organization Name").NotNull().WithMessage("Please Enter Organization Name");
        }
    }
}
