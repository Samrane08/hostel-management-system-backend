using Model.Common;

namespace Service.Interface;

public interface ISMSService
{
    Task<bool> SendSMS(SMSParam model);
}
