using FluentValidation;

namespace Hutech.Models
{
    public class ActivityViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class ActivityValidator : AbstractValidator<ActivityViewModel>
    {
        public ActivityValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter Activity Name");
        }
    }
}
