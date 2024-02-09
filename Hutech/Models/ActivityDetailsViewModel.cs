
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
        public long InstrumentId { get; set; }
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
        public string? DeaprtmentName { get; set; }
        public string Remark { get; set; }
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
        public long InstrumentId { get; set; }
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
        public string? DeaprtmentName { get; set; }
        public string Remark { get; set; }
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
        public List<IFormFile> Files { get; set; }
        public List<FileViewModel>? UplodedFile { get; set; }
    }
    public class ActivityDetailsValidator : AbstractValidator<ActivityDetailsViewModel>
    {

        public ActivityDetailsValidator()
        {
            RuleFor(x => x.InstrumentId).NotEmpty().WithMessage("Please Select InstrumentId");
            RuleFor(x => x.InstrumentActivityId).NotEmpty().WithMessage("Please Select Instrument ActivityId");
            RuleFor(x => x.Remark).NotEmpty().WithMessage("Please enter Remark");
        }
    }
}
