﻿using Model;

namespace Service.Interface;

public interface IProfileService
{
    Task<PersonalDetailsModel> GetPersonalDetails();
    Task<PersonalDetailsModel> SavePersonalDetails(PersonalDetailsModel model);
    Task<bool> DoumentUpload(DocumentUploadModel model);
    Task<AddressDetailsModel> SaveAddressDetails(AddressDetailsModel model);
    Task<AddressDetailsModel> GetAddressDetails();
    Task<ParentAddressModel> GetParentAddressDetails();
    Task<ParentAddressModel> SaveParentAddressDetails(ParentAddressModel model);
    Task<List<HostelPreferenceModel>> GetHostelPreference(int? Lang);
    Task SaveHostelPreference(List<HostelPreferenceModel> models);
    Task<OtherDetailsModel> SaveOtherDetails(OtherDetailsModel model);
    Task<OtherDetailsModel> GetOtherDetails();
    Task<List<ViewDocumentModel>> GetUploadDocuments();
    Task<List<SelectListModel>> GetHostelList(int? Lang);
    Task<PreviewModel> Preview();
    Task<ApplicationModel?> HostelApply(string? ApplicationId);
    Task<ProfileCompleteValidationModel> ValidateProfile();
    Task<ProfileCompleteValidationModel> ProfileStatus();
    Task<ApplicationModel?> GetPaymentParams();
    Task<List<ApplicationMainModel>?> GetApplicationList();
    Task<string?> CancelApplication(string ApplicationNo);
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
}