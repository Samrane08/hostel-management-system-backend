using Model;

namespace Service.Interface
{
    public interface IEmailTemplateService
    {
        Task<List<EmailTemplateModel>> GetListAsync();
        Task UpsertAsync(EmailTemplateModel model);
        Task<EmailTemplateModel> GetByKeyAsync(string key);
    }
}
