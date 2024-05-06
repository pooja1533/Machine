
using DocumentFormat.OpenXml.Office.CoverPageProps;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hutech.Models
{
    public class ActivityDetailsViewModel
    {
        public ActivityDetailsViewModel() 
        {
            InstrumentNameId = new List<SelectListItem>();
            InstrumentsActivity = new List<SelectListItem>();
            User = new List<SelectListItem>();
            ActivityDetails=new List<ActivityDetailsViewModel>();
        }
        public long Id { get; set; }
        public long InstrumentActivityId { get; set; }
        public long? InstrumentId { get; set; }
        public List<SelectListItem> InstrumentNameId { get; set; }
        public List<SelectListItem> InstrumentsActivity { get; set; }
        public List<SelectListItem> User { get; set; }
        public string? InstrumentName { get; set; }
        public string InstrumentSerial { get; set; }
        public string? Model { get; set; }
        //public string InstrumentsId { get; set; }
        public string LocationName { get; set; }
        public string TeamName { get; set; }
        public string TeamLocation { get; set; }
        public int Days { get; set; }
        public string Frequency { get; set; }
        public string? RequirementName { get; set; }
        public string? DepartmentName { get; set; }
        public string? Remark { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime PerformedDate { get; set; }
        public string myeumjson
        {
            get
            {
                return JsonConvert.SerializeObject(Enum.GetValues(typeof(FrequencyEnumData)), new Newtonsoft.Json.Converters.StringEnumConverter());
            }
            set { }
        }
        public List<ActivityDetailsViewModel> ActivityDetails { get; set; }
        public string? UserId { get; set; }
        [NotMapped]
        public string? DocumentId { get; set; }
        [NotMapped]
        public string? Path { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalPages { get; set; }
        public int? CurrentPage { get; set; }
        [NotMapped]
        public string? InstrumentIdName { get; set; }
        public int SelectedStatus { get; set; }
        public List<SelectListItem>? Status { get; set; }
        [NotMapped]
        public string? fullname { get; set; }
        [NotMapped]
        public string? Role { get; set; }
        public string? UpdatedBy { get; set; }
        public string CreatedByUserId { get; set; }

    }
    public enum FrequencyEnumData
    {
        FrequencyDay = 1,
        FrequencyMonth = 2,
        FrequencyYear = 3,
    }
    public class ActivityDetailsDocumentViewModel
    {
        public ActivityDetailsDocumentViewModel()
        {
            InstrumentNameId = new List<SelectListItem>();
            InstrumentsActivity = new List<SelectListItem>();
            User = new List<SelectListItem>();
            ActivityDetails = new List<ActivityDetailsViewModel>();
            UplodedFile = new List<FileViewModel>();
        }
        public long Id { get; set; }
        public long InstrumentActivityId { get; set; }
        public long? InstrumentId { get; set; }
        public List<SelectListItem> InstrumentNameId { get; set; }
        public List<SelectListItem> InstrumentsActivity { get; set; }
        public List<SelectListItem> User { get; set; }
        public string InstrumentName { get; set; }
        public string InstrumentSerial { get; set; }
        public string Model { get; set; }
        //public string InstrumentsId { get; set; }
        public string LocationName { get; set; }
        public string TeamName { get; set; }
        public string TeamLocation { get; set; }
        public int Days { get; set; }
        public string Frequency { get; set; }
        public string? RequirementName { get; set; }
        public string? DepartmentName { get; set; }
        public string? Remark { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime PerformedDate { get; set; }
        public string myeumjson
        {
            get
            {
                return JsonConvert.SerializeObject(Enum.GetValues(typeof(FrequencyEnumData)), new Newtonsoft.Json.Converters.StringEnumConverter());
            }
            set { }
        }
        public List<ActivityDetailsViewModel> ActivityDetails { get; set; }
        public string? UserId { get; set; }
        public List<IFormFile>? Files { get; set; }
        public List<FileViewModel>? UplodedFile { get; set; }
    }
    public class ActivityDetailsValidator : AbstractValidator<ActivityDetailsViewModel>
    {

        public ActivityDetailsValidator()
        {
            RuleFor(x => x.InstrumentId).NotEmpty().WithMessage("Please Select InstrumentId").NotNull().WithMessage("Please Select InstrumentId");
            RuleFor(x => x.InstrumentActivityId).NotEmpty().WithMessage("Please Select Instrument ActivityId").NotNull().WithMessage("Please Select Instrument ActivityId");
        }
    }
    public class ActivityDetailsDocumentValidator : AbstractValidator<ActivityDetailsDocumentViewModel>
    {

        public ActivityDetailsDocumentValidator()
        {
            RuleFor(x => x.InstrumentId).NotEmpty().WithMessage("Please Select InstrumentId").NotNull().WithMessage("Please Select InstrumentId");
            RuleFor(x => x.InstrumentActivityId).NotEmpty().WithMessage("Please Select Instrument ActivityId").NotNull().WithMessage("Please Select Instrument ActivityId");
        }
    }
    public class ActivityDetailModel
    {
        public string? InstrumentIdName { get; set; }
        public string? InstrumentName { get; set; }
        public string? InstrumentSerial { get; set; }
        public string? Model {  get; set; }
        public string? Location { get; set; }
        public string? Department { get; set; }
        public string? UserId { get; set; }  
        public string? UpdatedBy { get; set; }
        public int PageNumber { get; set; }
        public string? Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
