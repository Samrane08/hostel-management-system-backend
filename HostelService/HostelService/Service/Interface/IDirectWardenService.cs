using Model;
using Model.LotteryModel;

namespace Service.Interface;

public interface IDirectWardenService
{
    Task<List<DirectWardenVacancyModel>> GetDirectWardenData(int? HostelID);
    Task<int> PostDirectWardenData(List<DirectWardenVacancyModel> model, int? HostelID);

    Task<List<DropDownData>> GetPriorityQuotaData();
}
