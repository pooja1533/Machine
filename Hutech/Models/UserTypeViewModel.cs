using FluentValidation;

namespace Hutech.Models
{
    public class UserTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string? ModifiedByUserId { get; set; }
        public DateTime? DateModifiedUtc { get; set; }
        public bool IsDeleted { get; set; } 
        public bool IsActive { get; set; }
    }
    public class UserTypeValidator : AbstractValidator<UserTypeViewModel>
    {
        public UserTypeValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter User type name");
        }
    }
}
