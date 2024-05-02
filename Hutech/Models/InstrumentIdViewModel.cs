using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hutech.Models
{
    public class InstrumentIdViewModel
    {
        public InstrumentIdViewModel()
        {
            Instruments = new List<SelectListItem>();
            Locations= new List<SelectListItem>();
            Teams = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
        }
        public long Id { get; set; }
        public string? InstrumentsId { get; set; }
        public string? Model  { get; set; }
        public string? InstrumentSerial { get; set; }
        public List<SelectListItem> Instruments { get; set; }
        public List<SelectListItem> Locations { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Teams { get; set; }
        public long? InstrumentId { get; set; }
        public long? LocationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? TeamId { get; set; }
        public string TeamLocation { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public string? LocationName { get; set; }
        [NotMapped]
        public string? InstrumentName { get; set; }
        [NotMapped]
        public string? TeamName { get; set; }
        public string? CreatedByUserId { get; set; }
        public string? ModifiedByUserId { get; set; }
        public DateTime DatecreatedUtc { get; set; }
        public DateTime DateModifiedUtc { get; set; }
        [NotMapped]
        public string? FullName { get; set; }
        [NotMapped]
        public string? Role { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? InstallationDate { get; set; }
        //public List<IFormFile>? Files { get; set; }
        [NotMapped]
        public string? DocumentId { get; set; }
        [NotMapped]
        public string? Path { get; set; }
    }
    public class InstrumentIdViewModelValidator : AbstractValidator<InstrumentIdViewModel>
    {
        public InstrumentIdViewModelValidator()
        {
            RuleFor(x => x.InstrumentsId).NotEmpty().WithMessage("Please enter InstrumentsId");
            RuleFor(x => x.Model).NotEmpty().WithMessage("Please enter Model");
            RuleFor(x => x.InstrumentSerial).NotEmpty().WithMessage("Please enter InstrumentSerial");
            RuleFor(x => x.InstrumentId).NotNull().WithMessage("Please select Instrument").NotEmpty().WithMessage("Please select Instrument");
            RuleFor(x => x.LocationId).NotNull().WithMessage("Please select Location").NotEmpty().WithMessage("Please select Location");
            RuleFor(x => x.TeamId).NotNull().WithMessage("Please select Team").NotEmpty().WithMessage("Please select Team");
            RuleFor(x => x.DepartmentId).NotNull().WithMessage("Please Select Department").NotEmpty().WithMessage("Please Select Department");
        }
    }
    public class InstrumentIdDocumentViewModelValidator : AbstractValidator<InstrumentIdDocumentViewModel>
    {
        public InstrumentIdDocumentViewModelValidator()
        {
            RuleFor(x => x.InstrumentsId).NotEmpty().WithMessage("Please enter InstrumentsId");
            RuleFor(x => x.Model).NotEmpty().WithMessage("Please enter Model");
            RuleFor(x => x.InstrumentSerial).NotEmpty().WithMessage("Please enter InstrumentSerial");
            RuleFor(x => x.InstrumentId).NotNull().WithMessage("Please select Instrument").NotEmpty().WithMessage("Please select Instrument");
            RuleFor(x => x.LocationId).NotNull().WithMessage("Please select Location").NotEmpty().WithMessage("Please select Location");
            RuleFor(x => x.TeamId).NotNull().WithMessage("Please select Team").NotEmpty().WithMessage("Please select Team");
            RuleFor(x => x.DepartmentId).NotNull().WithMessage("Please Select Department").NotEmpty().WithMessage("Please Select Department");
        }
    }
    public class InstrumentIdModel
    {
        public string? InstrumentIdName { get; set; }
        public string? Model { get; set; }
        public string? InstrumentName { get; set; }
        public string? InstrumentSerial { get; set; }
        public string? InstrumentLocation {  get; set; }
        public string? TeamName { get; set; }
        public string? UpdatedBy { get; set; }
        public int PageNumber { get; set; }
        public string? Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
    public class InstrumentIdDocumentViewModel
    {
        public InstrumentIdDocumentViewModel()
        {
            Instruments = new List<SelectListItem>();
            Locations = new List<SelectListItem>();
            Teams = new List<SelectListItem>();
            Departments = new List<SelectListItem>();
            InstrumentIds=new List<InstrumentIdViewModel>();
            UplodedFile = new List<FileViewModel>();
        }
        public long Id { get; set; }
        public string? InstrumentsId { get; set; }
        public string? Model { get; set; }
        public string? InstrumentSerial { get; set; }
        public List<SelectListItem> Instruments { get; set; }
        public List<SelectListItem> Locations { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<SelectListItem> Teams { get; set; }
        public long? InstrumentId { get; set; }
        public long? LocationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? TeamId { get; set; }
        public string TeamLocation { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public string? LocationName { get; set; }
        [NotMapped]
        public string? InstrumentName { get; set; }
        [NotMapped]
        public string? TeamName { get; set; }
        public string? CreatedByUserId { get; set; }
        public string? ModifiedByUserId { get; set; }
        public DateTime DatecreatedUtc { get; set; }
        public DateTime DateModifiedUtc { get; set; }
        [NotMapped]
        public string? FullName { get; set; }
        [NotMapped]
        public string? Role { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? InstallationDate { get; set; }
        public List<IFormFile>? Files { get; set; }
        public List<FileViewModel>? UplodedFile { get; set; }
        public List<InstrumentIdViewModel> InstrumentIds { get; set; }
    }
}
