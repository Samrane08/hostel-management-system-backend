using Model;

namespace Service.Interface
{
    public interface IReportService
    {
        Task<TableResponseModel<ApplicationModel>> GetListAsync(ReportFilterModel model);
        Task<List<Dictionary<string, object>>> GetDailyReports(int? reportId);
        Task<List<Dictionary<string, object>>> GetListAsyncForWarden();
    }
}
