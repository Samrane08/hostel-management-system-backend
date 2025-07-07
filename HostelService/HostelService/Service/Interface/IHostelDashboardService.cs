using System.Threading.Tasks;
using Model;

namespace Service.Interface
{
    public interface IHostelDashboardService
    {
        Task<HostelDashboardModel?> GetHostelDashboard(int? ServiceType, int? Installment, int? academicyear);
        Task<HostelDashboardModel?> GetBADashboardApplication(int? division, int? district, int? applicationServiceType);
        Task<List<SelectListModel>> GetBATileCourseTypeDetailsCount(ApplicationServiceType model);
        //Task<List<SearchApplicationReponseModel>> GetHostelsApplications(string flag);
        Task<List<SelectListModel>> GetApplicationFilters();

        Task<List<SelectListModel>> GetscrutinylevelFilters(int? IsServiceType, int? IsApplicantnewExisting);

        Task<List<HostelDashboardModel>> GetBASearchFilter(SearchFilter model);

        //Task<List<BASearchedDataresponseModel>> GetBASearchFilterCountdata(SearchFilter1 model);
        Task<object> GetBASearchFilterCountdata(SearchFilter1 model);
        Task<List<SelectListAttendence>> GetapplicationNumberFilter(int? ayid);

        Task<List<SelectListModel>> GetInstallmentlevelFilters(int? IsServiceType);

    }
}
