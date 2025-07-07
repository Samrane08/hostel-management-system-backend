using Helper;
using Repository.Data;
using Repository.Entity;
using Service.Interface;

namespace Service.Implementation;

public class ExceptionLogger: IExceptionLogger
{
    private readonly ApplicationDbContext context;

    public ExceptionLogger(ApplicationDbContext context)
    {
        this.context = context;
    }
    public async Task LogTrack(string location,string exception)
    {
        try
        {
            var entity = new ErrorLogger()
            {
                LoggedAt = location,
                Exception = exception,
                Status = Repository.Enums.Status.Active
            };
            await context.ErrorLogger.AddAsync(entity);
            await context.SaveChangesAsync();   
        }
        catch (Exception ex)
        {

            ExceptionLogging.LogException(Convert.ToString(ex));
            throw;
        }
    }
}
