using MimeKit;
using System.Linq;

namespace Hutech.API
{
    public interface IEmailSenderService
    {
        void SendEmail(MessageServiceModel message);
        Task SendEmailAsync(MessageServiceModel message);
    }
    public class MessageServiceModel
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public IFormFileCollection? Attachments { get; set; }

        public MessageServiceModel(IEnumerable<string> to, string subject, string content, IFormFileCollection? attachments = null)
        {
            To = new List<MailboxAddress>();
            To = to.Select(d => new MailboxAddress(d, d)).ToList();
            Subject = subject;
            Content = content;
            Attachments = attachments;
        }
    }
}
