using FluentValidation;

namespace Hutech.Models
{
    public class InstrumentViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }    
        public bool IsActive { get; set; }  
        public bool IsDeleted { get; set; }
    }
    public class InstrumentValidator:AbstractValidator<InstrumentViewModel>
    {

        public InstrumentValidator() 
        { 
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Please Enter Name")
            .Matches(@"^[A-Za-z\s]*$").WithMessage("Name should only contain letters.");
        }
    }
}
