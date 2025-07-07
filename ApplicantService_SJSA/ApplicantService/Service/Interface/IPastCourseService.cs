using Model;

namespace Service.Interface;

public interface IPastCourseService
{
    Task<int> SaveAsync(EducationDetails model);
    Task<int> UpdateAsync(ApplicantEducationDetails model);
    Task<List<CourseDetailsTblViewModel>>GetAsync ();
    Task<int> DeleteAsync(long rowId);

    Task<int> SavePrePast(ApplicantPreschoolRecordPast model);
    Task<List<ApplicantPreSchoolRecordView>> GetPrePast();
    Task<int> DeletePrePast(long Id);
    Task<string> CheckIsPreOrPost();
    Task<string> GetMininumEntryYear();
    


}
