using AutoMapper.Internal;

namespace Hutech.API
{
    public interface IMailService
    {
        void SendEmailAsync(MessageServiceModel mailRequest);
    }
}
