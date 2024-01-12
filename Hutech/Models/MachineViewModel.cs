using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Hutech.Models
{
    public class MachineViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public string Vednor { get; set; }
        public int Warranty { get; set; }
        public int ServiceInterval { get; set; }
        //public int ServiceIntervalTime { get; set; }
        public Interval? Interval { get; set; }
        public string? Comment { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? CommentDate { get; set; }
    }
    public class MachineValidator : AbstractValidator<MachineViewModel>
    {
        public MachineValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please Enter Machine Name");
            RuleFor(x => x.PurchaseDate).NotEmpty().WithMessage("Please Enter Machine purchase date");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be Greater then zero");
            RuleFor(x => x.Brand).NotEmpty().WithMessage("Please Enter Brand");
            RuleFor(x => x.Vednor).NotEmpty().WithMessage("Please Enter Brand");
            RuleFor(x => x.Warranty).GreaterThan(0).WithMessage("Warranty must be Greater then zero");
            RuleFor(x => x.ServiceInterval).GreaterThan(0).WithMessage("Service Interval must be greater than zero");
        }
    }
    public enum Interval
    {
        Day = 1,
        Month = 2,
        Year = 3,
    }
}
