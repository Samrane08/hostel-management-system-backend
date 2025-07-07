using Model;

namespace Service.Interface
{
    public interface IScrutinyService
    {
        Task<SrutinyResultModel> Scrutiny(ScrutinyModel model);
        Task<List<ActionModel>> Workflow(long ApplicationId);
        Task<List<SrutinyRemarkModel>> History(long ApplicationId);
    }
}
