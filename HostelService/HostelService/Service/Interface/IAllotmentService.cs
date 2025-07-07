using Model;

namespace Service.Interface
{
    public interface IAllotmentService
    {
        Task<TableResponseModel<AllotWaitApplicationModel>> GetListAsync(SearchAllotmentStatusModel model);
        Task<TableResponseModel<AllotmentModel>> GetAllListAsync(SearchAllotmentStatusModel model);
        Task<List<AllotmentCountModel>> GetCourseWiseList(int? HostelId, int? courseID);
        Task<List<AllotmentCountModel>> GetCategoryWiseList(int? HostelId, int? courseID);
        Task<List<CastwiseCountModel>> GetCasteWiseList(int? HostelId, int? courseID, int? casteID);
        Task<List<SelectListModel>> GetHostelList();
        Task<List<SelectListModel>> GetHostelListDistrictWiseDeleteSjsaAadhaar(int? DistrictId);
    }
}
