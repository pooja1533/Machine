using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hutech.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            UserTypesId = new List<SelectListItem>();
            LocationsId = new List<SelectListItem>();
            DepartmentsId = new List<SelectListItem>();
            UserRolesId = new List<SelectListItem>();
            WindowsUserNamesId=new List<SelectListItem>();
        }
        public Guid Id { get; set; }
        public string AspNetUserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string EmployeeId { get; set; }
        public string? Remark {  get; set; }
        public int LoginValidityDays {  get; set; }
        public long UserTypeId { get; set; }
        public List<SelectListItem> UserTypesId { get; set; }
        public long LocationId { get; set; }
        public List<SelectListItem>LocationsId { get; set; } 
        public long DepartmentId { get; set; }
        public List<SelectListItem> DepartmentsId { get; set; }
        public long UserRoleId { get; set; }
        public List<SelectListItem> UserRolesId { get; set; }
        public string? WindowsUserName { get; set; }
        public List<SelectListItem> WindowsUserNamesId { get; set; }
        public List<string> SelectedUserRoleId { get; set; }
        [NotMapped]
        public string? FullName { get; set; }
        [NotMapped]
        public string? DepartmentName { get; set; }
        [NotMapped]
        public string? LocationName { get; set; }
        [NotMapped]
        public string? RoleName { get; set; }
        [NotMapped]
        public string? UserType { get; set; }
        public int? UserstatusId { get; set; }
        [NotMapped]
        public long UserId { get; set; }
        [NotMapped]
        public string? UserstatusName { get; set; }
        public string? RemarkForReject { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? RejectedBy { get; set; }
        public DateTime? RejectedDate { get; set; }
    }
    public class UserValidator : AbstractValidator<UserViewModel>
    {
        public UserValidator()
        {
            RuleFor(p => p.FirstName).NotNull().WithMessage("Please enter First Name").NotEmpty().WithMessage("Please enter First Name");
            RuleFor(x => x.LastName).NotNull().WithMessage("Please enter Last Name").NotEmpty().WithMessage("Please enter Last Name");
            RuleFor(x => x.EmployeeId).NotNull().WithMessage("Please enter EmployeeId").NotEmpty().WithMessage("Please enter EmployeeId").Matches(@"^[a-zA-Z0-9]{24}$").WithMessage("EmployeeId must be 24 character and contain only alphabets and numbers");
            
            //RuleFor(s => s.Email).NotEmpty().WithMessage("Plaese enter Email address ")
            //                     .EmailAddress().WithMessage("Email address is not valid");
            RuleFor(x => x.UserTypeId).NotNull().NotEmpty().WithMessage("Please select User Type");

            RuleFor(x => x.WindowsUserName).NotNull().NotEmpty().WithMessage("Please select Windows UserName").When(x => x.UserType == "clintha").WithMessage("Please select Windows UserName");
            RuleFor(x => x.LocationId).NotNull().NotEmpty().WithMessage("Please select Location");
            RuleFor(x => x.DepartmentId).NotNull().NotEmpty().WithMessage("Please select Department");
            RuleFor(x => x.SelectedUserRoleId).NotNull().NotEmpty().WithMessage("Please select User Role");
        }
    }
    public class UserModal
    {
        public string? FullName { get; set; }
       public string? UserName { get; set; }
        public string? EmailId { get; set; }
        public int PageNumber { get; set; }
        public string? status { get; set; }
        public string? LoggedInUserId { get; set; }
        public string? EmployeeId { get; set; }
        public string? UserType { get; set; }
        public string? Department { get; set; }
        public string? Role { get; set; }
        public string? Location { get; set; }
    }
}
