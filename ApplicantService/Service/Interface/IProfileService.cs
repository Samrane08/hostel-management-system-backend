using Model;

namespace Service.Interface;

public interface IProfileService
{
    Task<PersonalDetailsModel> GetPersonalDetails();
    Task<PersonalDetailsModel> SavePersonalDetails(PersonalDetailsModel model);
    Task<bool> DoumentUpload(DocumentUploadModel model);
    Task<bool> InstallmentDocumentUpload(DocumentUploadModel model);
    Task<bool> InstallmentDocFinalSubmit(InstallmentDocFinalSubmitModel model);
    Task<string> SaveAddressDetails(AddressDetailsModel model);
    Task<AddressDetailsModel> GetAddressDetails(int caller);
    Task<ParentAddressModel> GetParentAddressDetails(int caller);
    Task<string> SaveParentAddressDetails(ParentAddressModel model);
    Task<List<HostelPreferenceModel>> GetHostelPreference(int? Lang);
    Task SaveHostelPreference(List<HostelPreferenceModel> models);
    Task<string> SaveOtherDetails(OtherDetailsModel model);
    Task<OtherDetailsModel> GetOtherDetails();
    Task<List<ViewDocumentModel>> GetUploadDocuments(int? deptId);
    Task<List<SelectListModel>> GetHostelList(int? Lang);
    Task<PreviewModel> Preview();
    Task<ApplicationModel?> HostelApply();
    Task<ProfileCompleteValidationModel> ValidateProfile();
    Task<ProfileCompleteValidationModel> ProfileStatus();
    Task<ApplicationModel?> GetPaymentParams();
    Task<List<ApplicationMainModel>?> GetApplicationList();
   
    Task<int?> Acceptallocation(HostelAcceptanceModel model);
    Task<CallLetterResponseModel?> DownloadCallLetter(int applicationId);
    Task<double?> GetMostRecentCoursepercentage();
    Task<PreviewModel> ApplicationDetails(long AppId);
    Task ApplicationBackup(long ApplicationId);
    Task SaveHostelPreferenceV2(List<HostelPreferenceModel> models);
	Task<List<ApplicantStatusResponseModel>> GetApplicationStatusAtDesk(int appln);

    Task<bool> CheckApplicationAlreadyExistForService(int? serviceType);
    Task<string> CheckValidationForSwadharApply ();
    Task<OfflineApplicationModel?> GetOfflineApplication();
    Task<AdmittedApplicationModel?> GetAdmittedApplication();
    Task<ApplicationInstallmentModel> GetApplicationInstallmentDetails();
    DbtApplicantStatusModel CheckApplicantDataAvailbility(string academicYear);
    int? InsertApplicantPrefilledData(string aadharRefNo, string academicYear);

    Task<string> ValidatePreviewData();
    Task<List<ApplicationCommonModel>?> GetApplicationListAtCommonDesk();
    Task<bool> getCastewiseElgIbility(int? casteCategoruId);
    Task<string?> CancelApplication(string ApplicationNo);
    Task<string?> CancelApplicationByApplicant(Int64 Id);
}