using Model;

namespace Service.Interface
{
    public interface ISetVacencyService
    {       
        Task<List<FetchVacancyResponse>?> FetchVacancyListAsync(int TypeOfCourseId);
        Task<List<FetchVacancyQuotaTypeResponse>?> FetchQuotaTypeAsync();
        Task<FetchVacancyCourseTypeResponse?> FetchCourseTypeAsync();
        Task UpsertVacancyAsync(VacencySetModel model);
        Task UpsertCourseTypeVacancyAsync(CourseTypeSetVacancyModel model);
        Task<string> InsertQuotaTypeVacancyAsync(QuotaTypeObjectClass model);
        Task<List<SelectListModel>> FetchAllQuotaDropDownAsync();
        Task<CapapcitySetModel?> CheckIfCapacitiesSetAsync();
        Task<CapapcitySetModel?> CheckIfCapacitiesSetAsyncVjnt(int hostelId);
        Task<List<FetchVacancyQuotaTypeResponse>?> FetchQuotaTypeAsyncVjnt(int hostelId);
        Task<FetchVacancyCourseTypeResponse?> FetchCourseTypeAsyncVjnt(int hostelId);
        Task<List<FetchVacancyResponse>?> FetchVacancyListAsyncVjnt(int hostelId,int TypeOfCourseId);
        Task<string> InsertQuotaTypeVacancyAsyncVjnt(int hostelId,QuotaTypeObjectClass model);
        Task UpsertVacancyAsyncVjnt(VacencySetModel model);


    }
}
