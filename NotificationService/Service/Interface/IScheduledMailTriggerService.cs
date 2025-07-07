using Model;

namespace Service.Interface
{
    public interface IScheduledMailTriggerService
    {
        Task EmailSend(EmailSender3 model);
        Task ScheduledMailTrigger();
    }
}
