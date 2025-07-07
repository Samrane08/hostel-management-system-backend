using Model.Common;

namespace Service.Interface;

public interface IEmailService
{
    Task<bool> SendAsync(EmailParam param);
}
