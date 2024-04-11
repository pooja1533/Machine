using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hutech.Models
{
    public class InstrumentViewModel
    {     
        public long Id { get; set; }
        public string Name { get; set; }    
        public bool IsActive { get; set; }  
        public bool IsDeleted { get; set; }
        //public List<IFormFile> Files { get; set; }
        [NotMapped]
        public string? DocumentId { get; set; }
        [NotMapped]
        public string? Path { get; set; }
        public string? CreatedByUserId { get; set; }
        public string? ModifiedByUserId { get; set; }
        public DateTime DatecreatedUtc { get; set; }
        public DateTime DateModifiedUtc { get; set; }
        [NotMapped]
        public string? FullName { get; set; }
        [NotMapped]
        public string? Role { get; set; }
    }
    public class InstrumentDocumentViewModel
    {
        public InstrumentDocumentViewModel() 
        {
            UplodedFile = new List<FileViewModel>();
        }
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<IFormFile>? Files { get; set; }
        public List<FileViewModel>? UplodedFile { get; set; }
        public string? FileName { get; set; }

    }
    public class FileViewModel
    {
        public long Id { get; set; }
        public String FileName { get; set; }
        public string FilePath { get; set; }
    }
    public class InstrumentValidator:AbstractValidator<InstrumentDocumentViewModel>
    {

        public InstrumentValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please Enter Name of Instrument");
        }
    }
    public class InstrumentModel
    {
        public string? InstrumentName { get; set; }
        public string? UpdatedBy { get; set; }
        public int PageNumber { get; set; }
        public string? Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
