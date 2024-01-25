using FluentValidation;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        }
        public long Id { get; set; }
        public string InstrumentsId { get; set; }
        public string Model  { get; set; }
        public string InstrumentSerial { get; set; }
        public List<SelectListItem> Instruments { get; set; }
        public List<SelectListItem> Locations { get; set; }
        public List<SelectListItem> Teams { get; set; }
        public long InstrumentId { get; set; }
        public long LocationId { get; set; }
        public long TeamId { get; set; }
        public string TeamLocation { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public string? LocationName { get; set; }
        [NotMapped]
        public string? InstrumentName { get; set; }
        [NotMapped]
        public string? TeamName { get; set; }
    }
    public class InstrumentIdViewModelValidator : AbstractValidator<InstrumentIdViewModel>
    {
        public InstrumentIdViewModelValidator()
        {
            RuleFor(x => x.InstrumentsId).NotEmpty().WithMessage("Please enter InstrumentsId");
            RuleFor(x => x.Model).NotEmpty().WithMessage("Please enter Model");
            RuleFor(x => x.InstrumentSerial).NotEmpty().WithMessage("Please enter InstrumentSerial");
        }
    }
}
