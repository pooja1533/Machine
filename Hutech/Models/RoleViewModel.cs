using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hutech.Models
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }
       public string? Name { get; set; }
        public DateTime DatecreatedUtc { get; set; }
        public string? CreatedByUserId { get; set; }
        public DateTime DateModifiedUtc { get; set; }
        public string? ModifiedByUserId { get; set; }
        [NotMapped]
        public string? fullname { get; set; }
        [NotMapped]
        public string? Role { get; set; }
    }
    public class RoleValidator : AbstractValidator<RoleViewModel>
    {
        public RoleValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please Enter Role Name");
        }
    }
    public class RoleModel
    {
        public string? roleName { get; set; }
        public string? updatedBy { get; set; }
        public int pageNumber { get; set; }
        public DateTime? updatedDate { get; set; }
    }
}
