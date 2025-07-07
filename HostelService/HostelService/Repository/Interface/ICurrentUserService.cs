namespace Repository.Interface;

public interface ICurrentUserService
{
    string UserId { get; }
    string HostelId { get; }
    string RoleEntityId { get; } //2 : Warden //3 :Department //6: State User //8:Desk User
    string DistrictId { get; }
    string WorkFlowId { get; }

    string DeptId { get; }
}
