using Model;
using Model.LotteryModel;

namespace Service.Interface
{
    public interface ILotteryService
    {
        Task<LotteryGlobalModel> GetData(int? DistrictId, int? CourseId, int? Hostelid);
        Task<List<HostelPreferenceModel>> GetApplicantPreference(long Userid);
        Task<HostelVacancyModel> GetHostelVacancy(int CourseId, int HostelId, int Priority);
        Task UpdateAllotment(long UserId, int HostelId, string ApplicationNo, int CourseType, int CasteCategoryId, int CasteId, int Priority);
        Task AllotmentGenerate(GenerateLotteryModel model);
        Task<int> GetMeritGenerationStatus( int? CourseId );
        Task<MeritGlobalModel> MeritGenerationReport(int? CourseId);
       Task GenerateGeneralMeritList(GenerateLotteryModel model);
        Task<TableResponseModel<MeritResponseModel>> GetMeritApproveList(MeritApproveModel model);
        Task<string> SaveMeritApproveList(List<MeritResponseModel> model);
        Task<TableResponseModel<MeritResponseModel>> GetMeritAsstRejectList(MeritRejectModel model);
        Task<string> SaveMeritAsstRejectList(List<MeritResponseModel> model);
    }
}
