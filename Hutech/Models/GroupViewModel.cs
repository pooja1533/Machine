using FluentValidation;

namespace Hutech.Models
{
    public class GroupViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class GroupValidator : AbstractValidator<GroupViewModel>
    {
        public GroupValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter Group Name")
            .Matches(@"^[A-Za-z\s]*$").WithMessage("Group Name should only contain letters.");
        }
    }
}
