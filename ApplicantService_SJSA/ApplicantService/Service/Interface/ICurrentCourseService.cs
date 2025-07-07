using Model;
using System.Dynamic;

namespace Service.Interface;

public interface ICurrentCourseService
{
    Task<string> SaveCurrentCourseDetailsAsync(CurrentCourseDetails model);
    Task<int> UpdateCurrentCourseDetailsAsync(CurrentCourseDetails model);
    Task<List<CourseDetailsTblViewModel>>GetCourseDetailsByUserId ();
    Task<string> DeleteCurrentCourseByRowId(long rowId);

    Task<ExpandoObject> IsCapRequired(int courseId);
    Task<string> CalculateYearGap(int courseYrId,string startYr);
    Task<string> CheckIsDatainYearOrder(int CourseId, int YearStudy);
    Task<List<personalInfo>> GetPersonalInfoByUserId();
    Task<List<personalInfo>> persona_reservedCategory();

    Task<List<SelectListModel>> Getqualificationtype(string caller);
    Task<string> GetCurrentCourseStartYear();
    Task<List<SelectListModel2>> PreSchools(string stateID,string districtID,string talukaID);
    Task<List<SelectListModel>> PreStandard(string caller);
    
   Task<int> SaveApplicantPreSchoolRecord(ApplicantPreSchoolRecord model);
    Task<List<ApplicantPreSchoolRecordView>> GetPreDetailsByUserId();
    Task<int> DeletePreCourseRecord(long Id);
    Task<List<SelectListModel>> GetCourseStatus(string _preOrPot, int _standardId);
    
    Task<List<SelectListModel>> GetCourseStatusYearByUserId(int yearId);

    Task<List<SelectListModel>> GetCurrentQualificationYear(int courseId, int? langID = 1);
    Task<bool> IsNewApplicant();
    Task<bool> CheckIsCurrentAvailable();
    Task<string> GetAadharAndCheckShouldFetchRecordFromPost(string Caller);
    Task<string> GetCourseDuration(int courseId);
    Task<List<SelectListModel>> TalukaList(int? distid, int? lang = 1);
    Task<List<SelectListModel>> DistList(int? state, int? lang = 1);


}
