using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hutech.Models
{
    public class ActivityViewModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string? CreatedByUserId { get; set; }
        public string? ModifiedByUserId { get; set; }
        public DateTime DatecreatedUtc { get; set; }
        public DateTime DateModifiedUtc { get; set; }
        [NotMapped]
        public string? FullName { get; set; }
        [NotMapped]
        public string? Role { get; set; }
    }
    public class ActivityValidator : AbstractValidator<ActivityViewModel>
    {
        public ActivityValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter Activity Name");
        }
    }
    public class ActivityModel
    {
        public string? ActivityName { get; set; }
        public string? UpdatedBy { get; set; }
        public int PageNumber { get; set; }
        public string? Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
