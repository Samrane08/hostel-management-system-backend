using Model;
using System.Threading.Tasks;
namespace Service.Interface;

public interface ILoginDetailsService
{
    Task<WebServiceUserModel> GetLoginDetails(long Userid);
    Task<WebServiceUserModel> GetLoginDetails(string AadhaarReferenceNo);
    Task<WebServiceUserModel> SaveLoginDetails(WebServiceUserModel model);
    Task<VerifiedStatusModel> VerifyStatus();
    Task<UIDResponseModel> UpsertAadhaarAsync(UIDResponseModel model);
    Task<UIDResponseModel> GetRegisterDetails();
    Task<WebServiceUserModel> SaveRegisterDetails(ProfileVerifyModel model);
    Task<bool> CheckEmailExist(string email);
    Task<bool> CheckMobileExist(string email);
    Task<List<SelectListModel>> HostelGenderWise(int dist, int? taluka);
    Task<string> GetAadharreferenceNumber();

    Task<string> UpdateOfflineUIDReference(string UID);
    Task<string> GetUIDNo();


}
