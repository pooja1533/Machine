using DocumentFormat.OpenXml.Office.CoverPageProps;
using FluentValidation;
using Hutech.Resources;

namespace Hutech.Models
{
    public class ConfigurationViewModel
    {
        public long Id { get; set; }
        public string FileSize { get; set; }
        public string FileType { get; set; }

    }
    public class ConfigurationValidator :AbstractValidator<ConfigurationViewModel>
    {
        public ConfigurationValidator()
        {
            RuleFor(x => x.FileType).NotEmpty().WithMessage("Please enter file types that you want to allow user to upload");

            RuleFor(x => x.FileSize).NotEmpty().WithMessage("Please enter File Size ");
        }
    }
}
