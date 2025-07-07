using Model;

namespace Service.Interface
{
    public interface IAccountService
    {
        Task<HostelProfileModel?> GetHostelProfile(string UserId,int? deptId);
        Task<DepartmentProfileModel?> GetDepartmentDetails(string UserId, int? EntityRole,int? deptId);
        Task<bool?> UpdateFirstLogin(string Id);
        Task<bool?> UpdateFirstLoginToTrue(string userlId);
        Task<bool> UpdateAadhaar(string UIDReference, string Name, int Gender);
    }
}
