namespace MasterService.Service.Interface
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string UserNumericId { get; }
        string DeptId {  get; }

    }
}
