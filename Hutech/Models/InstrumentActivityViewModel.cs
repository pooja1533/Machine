using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hutech.Models
{
    public class InstrumentActivityViewModel
    {
        public InstrumentActivityViewModel()
        {
            Instruments = new List<SelectListItem>();
            Activities= new List<SelectListItem>();
            Requirement= new List<SelectListItem>();
            Department= new List<SelectListItem>();
            EmailList = new List<SelectListItem>();
        }
        public long Id { get; set; }
        public List<SelectListItem> Instruments { get; set; }
        public List<SelectListItem> Activities { get; set; }
        public List<SelectListItem> Requirement { get; set; }
        public List<SelectListItem> Department { get; set; }
        public string? InstrumentActivityId { get; set; }
        public long? InstrumentId { get; set; }
        public long? ActivityId { get; set; }
        public long? RequirementId { get; set; }
        public long? DepartmentId { get; set; }
        public string InstrumentActivityName { get; set; }
        public int? Days { get; set; }
        public bool BeforeAlerts { get; set; }
        public List<SelectListItem>? EmailList { get; set; }
        public List<string>? SelectedEmailListInt { get; set; }
        public string? Frequency { get; set; }
        public int? FrequencyTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string MyEumJson
        {
            get
            {
                return JsonConvert.SerializeObject(Enum.GetValues(typeof(FrequencyEnum)), new Newtonsoft.Json.Converters.StringEnumConverter());
            }
            set { }
        }
        public string? InstrumentName { get; set; }
        public string? activityName { get; set; }
        public string? RequirementName { get; set; }
        public string? DeaprtmentName { get; set; }
        public int? BeforeAlertsTime { get; set; }
        [NotMapped]
        public string? SelectedGroups { get; set; }

        [NotMapped]
        public string? InstrumentActivityGroup { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string? CreatedByUserId { get; set; }
        public string? ModifiedByUserId { get; set; }
    }
    public class InstrumentActivityValidator : AbstractValidator<InstrumentActivityViewModel>
    {
        public InstrumentActivityValidator()
        {
            RuleFor(x => x.InstrumentActivityId).NotEmpty().WithMessage("Please enter InstrumentActivityId").NotNull().WithMessage("Please enter InstrumentActivityId");
            RuleFor(x => x.InstrumentId).NotNull().WithMessage("Please Select Instrument").NotEmpty().WithMessage("Please Select Instrument");
            RuleFor(x => x.ActivityId).NotNull().WithMessage("Please select Activity").NotEmpty().WithMessage("Please select Activity");
            RuleFor(x => x.Frequency).NotEmpty().WithMessage("Select Frequency").NotNull().WithMessage("Select Frequency");
            RuleFor(x => x.FrequencyTime).NotEmpty().WithMessage("Enter Frequency Time").NotNull().WithMessage("Enter Frequency Time");
            RuleFor(x => x.Days).NotEmpty().WithMessage("Please Enter Days").NotNull().WithMessage("Please Enter Days");
            RuleFor(x => x.RequirementId).NotNull().WithMessage("Please select Requirement").NotEmpty().WithMessage("Please select Requirement");
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("Please select Department").NotNull().WithMessage("Please select Department");
            RuleFor(x => x.SelectedEmailListInt).NotEmpty().WithMessage("Please select User group").NotNull().WithMessage("Please select User group");
        }
    }
    public enum FrequencyEnum
    {
        FrequencyDay = 1,
        FrequencyMonth = 2,
        FrequencyYear = 3,
    }
}
