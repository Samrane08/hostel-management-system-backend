using Model;

namespace Service.Interface
{
    public interface IOfflineRegistrationService
    {
        bool IsRecordValidate(OfflineAadhharList model);
        Task Registration(string JsonData);
        Task<List<OfflineRegistrationModel>> GetListAsync(int? HostelId);
        Task<List<OfflineAadhharList>> AdminQuery(string query, string caller);
        Task<int> UpdateAdminQuery(string query, string caller);
        Task<List<GetOfflineAadhharList>> GetListOffline(int? HostelId);
        Task<DirectVacancyCount?> DirectVacancyExistingValue();
        Task<List<GetOfflineAadhharList>> GetListOfflineExisting(int? HostelId);
        Task ExistingAadharRegistration(string JsonData, string FilePath);
        Task<TableResponseModel<ExistingAadharData>> GetListAsync(SearchExistingAadharDataModel model);
        Task<TableResponseModel<ExistingAadharData>> GetListAsync(SearchDeleteExistingAadharDataModel model);
        Task<string> DeleteExistingAadharRegistration(string HostelId);
        bool IsAadhaarValidate(string UIDNo);
        Task<string> DeleteExistingAadhaarId(int? Id, string? HostelId);
    }
}
