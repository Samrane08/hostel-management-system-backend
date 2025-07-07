using Model;
namespace Service.Interface;

public interface IEmailLogger
{
    Task<string> Log(EmailBroadcast model);
    Task Log(string TrackId);
    Task<string> EmailSend(EmailSender2 model);   
}