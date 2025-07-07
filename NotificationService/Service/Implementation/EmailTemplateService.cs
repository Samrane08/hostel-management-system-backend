using Helper;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Data;
using Repository.Entity;
using Service.Interface;

namespace Service.Implementation
{
    public class EmailTemplateService: IEmailTemplateService
    {
        private readonly ApplicationDbContext context;
        public EmailTemplateService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<List<EmailTemplateModel>> GetListAsync()
        {
            return await context.EmailTemplates.Select(x => x.ToType<EmailTemplateModel>()).ToListAsync();
        }
        public async Task UpsertAsync(EmailTemplateModel model)
        {
            try
            {
                var result = await context.EmailTemplates.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
                if (result != null)
                {
                    result.Subject = model.Subject;
                    result.Body = model.Body;
                }
                else
                {
                    var entity = model.ToType<EmailTemplate>();
                    entity.Body = model.Body;
                    entity.Status = Repository.Enums.Status.Active;
                    await context.EmailTemplates.AddAsync(entity);
                }
               await context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<EmailTemplateModel> GetByKeyAsync(string key)
        {
            return await context.EmailTemplates.Where(x=>x.Key == key).Select(x => x.ToType<EmailTemplateModel>()).FirstOrDefaultAsync();
        }
    }
}
