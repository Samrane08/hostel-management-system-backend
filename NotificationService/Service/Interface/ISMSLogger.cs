using Model;

namespace Service.Interface;

public interface ISMSLogger
{
    Task<string> Log(SMSBroadcast model);
    Task Log(string TrackId);
}
