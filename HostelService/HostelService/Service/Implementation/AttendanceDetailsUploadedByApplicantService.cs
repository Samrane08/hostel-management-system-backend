using Dapper;
using Helper;
using Model;
using Newtonsoft.Json;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation
{
    public class AttendanceDetailsUploadedByApplicantService : IAttendanceDetailsUploadedByApplicantService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;

        public AttendanceDetailsUploadedByApplicantService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService, IErrorLogger errorLogger)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }

        public async Task<TableResponseModel<AttendanceDetailsUploadedByApplicantModel>> GetApplicantAttendaceDetails(int pageIndex, int pageSize)
        {
            try
            {
                var response = new TableResponseModel<AttendanceDetailsUploadedByApplicantModel>();
                var result = new List<IEnumerable<dynamic>>();
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                
                var param = new DynamicParameters();
                param.Add("p_StartIndex", pageIndex, DbType.Int32);
                param.Add("p_PageSize", pageSize, DbType.Int32);
                param.Add("p_DistrictId", currentUserService.DistrictId, DbType.Int32);
                result = await dapper.MultiResult("usp_FetchSwadharAttendance", param, commandType: CommandType.StoredProcedure);

                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        List<AttendanceDetailsUploadedByApplicantModel> aadharData = new List<AttendanceDetailsUploadedByApplicantModel>();
                        foreach (var item in result[0].ToList())
                        {
                            AttendanceDetailsUploadedByApplicantModel objmodel1 = new AttendanceDetailsUploadedByApplicantModel();
                            objmodel1.ApplicantFullName = item.ApplicantFullName;
                            objmodel1.AcademicYear = item.AcademicYear;
                            objmodel1.InstallmentNumber = item.InstallmentNumber;
                            objmodel1.AttendanceDoc = item.AttendanceDoc;
                            objmodel1.AttendancePercentage = item.AttendancePercentage;
                            objmodel1.PercentageOfMarks = item.PercentageOfMarks;
                            objmodel1.isPostMatricScholarship = item.isPostMatricScholarship;
                            objmodel1.PostMatricScholarshipAmount = item.PostMatricScholarshipAmount;
                            aadharData.Add(objmodel1);
                        }

                        var data = JsonConvert.SerializeObject(aadharData);
                        response.List = JsonConvert.DeserializeObject<List<AttendanceDetailsUploadedByApplicantModel>>(data);
                    }
                    if (result[1] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[1].FirstOrDefault());
                        var paginator = JsonConvert.DeserializeObject<PaginationDataModel>(data);
                        if (paginator != null)
                        {
                            response.PageSize = paginator.PageSize;
                            response.PageIndex = paginator.PageIndex;
                            response.Total = paginator.Total;
                        }
                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                await errorLogger.Log("usp_FetchSwadharAttendance", ex);
                return new TableResponseModel<AttendanceDetailsUploadedByApplicantModel>() { List = new List<AttendanceDetailsUploadedByApplicantModel>(), PageIndex = 1, PageSize = 10, Total = 0 };
            }
        }
    }
}
