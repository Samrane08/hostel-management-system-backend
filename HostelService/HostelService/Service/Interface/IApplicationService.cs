using Model;

namespace Service.Interface;

public interface IApplicationService
{
    Task<TableResponseModel<ApplicationModel>> GetListAsync(SearchApplicationModel model);
    Task<TableResponseModel<ApplicationModel>> GetListOfflineAsync(SearchApplicationOfflineModel model);   
    Task<PreviewModel?> GetByIdAsync(long _AppId);
    Task<PreviewModel> GetByIdAsyncV2(long _AppId, int Param);
    Task<List<Object>> GetParams();
    Task<bool> ApplicationValidate(long _AppId);
    Task<bool> ApplicationFileValidate(long _AppId, string FileId);
}
