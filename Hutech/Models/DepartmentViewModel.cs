using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hutech.Models
{
    public class DepartmentViewModel
    {
        public string? Name { get; set; }
        public long Id { get; set; }
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
    public class DepartmentValidator : AbstractValidator<DepartmentViewModel>
    {
        public DepartmentValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter Department Name").NotNull().WithMessage("Please enter Department Name");
        }
    }
    public class DepartmentModel
    {
        public string? departmentName { get; set; }
        public string? updatedBy { get; set; }
        public string? status { get; set; }
        public DateTime? updatedDate { get; set; }
        public int pageNumber { get; set; }
    }
}
