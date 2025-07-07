using Model;

namespace Service.Interface;

public interface IHostelProfileService
{
    Task<HostelProfileModel?> GetHostelProfile();
    Task<HostelProfileModel?> GetHostelProfileByHostelId(int? hostelId);
    Task<HostelProfileModel?> SaveHostelProfile(HostelProfileModel model);
}
