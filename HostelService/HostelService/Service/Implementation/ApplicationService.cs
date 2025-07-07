using Dapper;
using Helper;
using Model;
using Newtonsoft.Json;
using Repository.Interface;
using Service.Interface;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Service.Implementation
{
    public class ApplicationService : IApplicationService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;
        public ApplicationService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService,IErrorLogger errorLogger)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }         
        public async Task<TableResponseModel<ApplicationModel>>  GetListAsync(SearchApplicationModel model)
        {
            try
            {
                var response = new TableResponseModel<ApplicationModel>();
                var param = new DynamicParameters();
                param.Add("p_AcademicYear", model.AYId, DbType.Int32);
                param.Add("p_CourseType", model.CourseType, DbType.Int32);
                param.Add("p_CasteCategory", model.CasteCategory, DbType.Int32);
                param.Add("p_Caste", model.Caste, DbType.Int32);
                param.Add("p_Status", model.Status, DbType.Int32);
                param.Add("p_CouseYear", model.CourseYear, DbType.Int32);
                param.Add("p_ApplicationNo", model.ApplicationNo, DbType.String);
                param.Add("p_Name", model.Name, DbType.String);
                param.Add("p_StartIndex", model.PageIndex, DbType.Int32);
                param.Add("p_PageSize", model.PageSize, DbType.Int32);
                param.Add("p_ServiceType", model.ServiceType, DbType.Int32);
                
                if (model.Status == -1) 
                   param.Add("p_IsNewApplicant", 0, DbType.Int32);
                else
                    param.Add("p_IsNewApplicant", model.IsNewApplicant, DbType.Int32);

                param.Add("p_District", !string.IsNullOrWhiteSpace(currentUserService.DistrictId) ? currentUserService.DistrictId : null, DbType.Int32);
                param.Add("p_WorkFlowId", !string.IsNullOrWhiteSpace(currentUserService.WorkFlowId) ? currentUserService.WorkFlowId : null, DbType.Int32);
                param.Add("p_InstallmentNo", 1, DbType.Int32);
                var result = new List<IEnumerable<dynamic>>();
                var dapper= dapperresolverInstance.Resolve(currentUserService.DeptId);
                if (!string.IsNullOrWhiteSpace(currentUserService.HostelId))
                {
                    param.Add("p_Hostelid", currentUserService.HostelId, DbType.Int32);
                    result = await dapper.MultiResult("usp_ScrutinyApplicationsWarden", param, commandType: CommandType.StoredProcedure);
                }
                else
                    result = await dapper.MultiResult("usp_ScrutinyApplicationsDeptNew", param, commandType: CommandType.StoredProcedure);

                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[0].ToList());
                        response.List = JsonConvert.DeserializeObject<List<ApplicationModel>>(data);
                     

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
                ExceptionLogging.LogException(Convert.ToString(ex));
                if (!string.IsNullOrWhiteSpace(currentUserService.HostelId))
                    await errorLogger.Log("usp_ScrutinyApplicationsWarden", ex);
                else
                    await errorLogger.Log("usp_ScrutinyApplicationsDeptNew", ex);
                return new TableResponseModel<ApplicationModel>() { List = new List<ApplicationModel>(), PageIndex = 1, PageSize = 10, Total = 0 };               
            }
        }
        public async Task<TableResponseModel<ApplicationModel>>  GetListOfflineAsync(SearchApplicationOfflineModel model)
        {
            try
            {
                var response = new TableResponseModel<ApplicationModel>();
                var param = new DynamicParameters();
                param.Add("p_AcademicYear", model.AcademicYear, DbType.Int32);
                param.Add("p_ApplicationNo", model.ApplicationNo, DbType.String);
                param.Add("p_Name", model.Name, DbType.String);
                param.Add("p_StartIndex", model.PageIndex, DbType.Int32);
                param.Add("p_PageSize", model.PageSize, DbType.Int32);
                var result = new List<IEnumerable<dynamic>>();
                if (!string.IsNullOrWhiteSpace(currentUserService.HostelId))
                {
                    int departmentId = Convert.ToInt32(currentUserService.DeptId);
                    var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                    if (departmentId == 1)
                        param.Add("p_Hostelid", !string.IsNullOrWhiteSpace(currentUserService.HostelId) ? currentUserService.HostelId : null, DbType.Int64);
                    else
                        param.Add("p_Hostelid", departmentId, DbType.Int64);
                    result = await dapper.MultiResult("usp_OfflineScrutinyApplicationsWarden", param, commandType: CommandType.StoredProcedure);
                }
                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[0].ToList());
                        response.List = JsonConvert.DeserializeObject<List<ApplicationModel>>(data);
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
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_OfflineScrutinyApplicationsWarden", ex);
                return new TableResponseModel<ApplicationModel>() { List = new List<ApplicationModel>(), PageIndex = 1, PageSize = 10, Total = 0 };
            }
        }
        public async Task<PreviewModel?> GetByIdAsync(long _AppId)
        {
            try
            {
                var response = new PreviewModel();
                var param = new DynamicParameters();
                param.Add("p_HostelId", !string.IsNullOrWhiteSpace(currentUserService.HostelId) ? currentUserService.HostelId : null, DbType.Int64);
                param.Add("p_ApplicationId", _AppId, DbType.Int64);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await dapper.MultiResult("usp_ScrutinyApplicationDetails", param, commandType: CommandType.StoredProcedure);
                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[0].FirstOrDefault());
                        response.AadharData = JsonConvert.DeserializeObject<AadhaarDetailsPreviewModel>(data);
                    }
                    if (result[1] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[1].FirstOrDefault());
                        response.PersonalDetails = JsonConvert.DeserializeObject<PersonalDetailsPreviewModel>(data);
                    }
                    if (result[2] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[2].FirstOrDefault());
                        response.CastDetails = JsonConvert.DeserializeObject<CastDetailsPreviewModel>(data);

                        var data2 = JsonConvert.SerializeObject(result[2].FirstOrDefault());
                        response.DomicileDetails = JsonConvert.DeserializeObject<DomicilePreviewModel>(data2);
                    }
                    if (result[3] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[3].FirstOrDefault());
                        response.PermanentAddress = JsonConvert.DeserializeObject<AddressDetailsPreviewModel>(data);
                    }
                    if (result[4] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[4].FirstOrDefault());
                        response.PresentAddress = JsonConvert.DeserializeObject<AddressDetailsPreviewModel>(data);
                    }
                    if (result[5] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[5].FirstOrDefault());
                        response.ParentAddress = JsonConvert.DeserializeObject<AddressDetails2PreviewModel>(data);
                    }
                    if (result[6] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[6].FirstOrDefault());
                        response.GuardianAddress = JsonConvert.DeserializeObject<AddressDetails2PreviewModel>(data);
                    }
                    if (result[7] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[7].FirstOrDefault());
                        response.OtherDetails = JsonConvert.DeserializeObject<OtherDetailsPreviewModel>(data);
                    }
                    if (result[8] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[8].ToList());
                        response.Preferences = JsonConvert.DeserializeObject<List<HostelPreferencePreviewModel>>(data);
                    }
                    if (result[9] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[9].ToList());
                        response.CurrentCourse = JsonConvert.DeserializeObject<List<CurrentCoursePreviewModel>>(data);
                    }
                    if (result[10] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[10].ToList());
                        response.PastCourse = JsonConvert.DeserializeObject<List<PastCoursePreviewModel>>(data);
                    }
                    if (result[11] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[11].ToList());
                        response.PreCurrentCourse = JsonConvert.DeserializeObject<List<PreCurrentCoursePreviewModel>>(data);
                    }
                    if (result[12] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[12].ToList());
                        response.PrePastCourse = JsonConvert.DeserializeObject<List<PrePastCoursePreviewModel>>(data);
                    }
                    if (result[13] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[13].ToList());
                        response.Documents = JsonConvert.DeserializeObject<List<DocumentPreviewModel>>(data);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }
        public async Task<PreviewModel> GetByIdAsyncV2(long _AppId, int Param)
        {
            try
            {
                var response = new PreviewModel();
                var param = new DynamicParameters();
                param.Add("p_HostelId", !string.IsNullOrWhiteSpace(currentUserService.HostelId) ? currentUserService.HostelId : null, DbType.Int64);
                param.Add("p_ApplicationId", _AppId, DbType.Int64);
                param.Add("p_Param", Param, DbType.Int64);
                param.Add("p_DeptId", currentUserService.DeptId, DbType.Int64);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                switch (Param)
                {
                    case 1:
                        response.PersonalDetails = await Task.FromResult(dapper.Get<PersonalDetailsPreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        break;
                    case 2:
                        response.CastDetails = await Task.FromResult(dapper.Get<CastDetailsPreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        break;
                    case 3:
                        response.DomicileDetails = await Task.FromResult(dapper.Get<DomicilePreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        break;
                    case 4:
                        response.PermanentAddress = await Task.FromResult(dapper.Get<AddressDetailsPreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        break;
                    case 5:
                        response.PresentAddress = await Task.FromResult(dapper.Get<AddressDetailsPreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        break;
                    case 6:
                        response.ParentAddress = await Task.FromResult(dapper.Get<AddressDetails2PreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        break;
                    case 7:
                        response.GuardianAddress = await Task.FromResult(dapper.Get<AddressDetails2PreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        break;
                    case 8:
                        response.OtherDetails = await Task.FromResult(dapper.Get<OtherDetailsPreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        break;
                    case 9:
                        response.Preferences = await Task.FromResult(dapper.GetAll<HostelPreferencePreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        break;
                    case 10:
                        var pcs = await Task.FromResult(dapper.GetAll<CurrentCoursePreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        if (pcs != null && pcs.Count > 0)
                        {
                            response.CurrentCourse = pcs;
                        }
                        else
                            response.PreCurrentCourse = await Task.FromResult(dapper.GetAll<PreCurrentCoursePreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        break;
                    case 11:
                        var pstc = await Task.FromResult(dapper.GetAll<PastCoursePreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        if (pstc != null && pstc.Count > 0)
                        {
                            response.PastCourse = pstc;
                        }
                        else
                            response.PrePastCourse = await Task.FromResult(dapper.GetAll<PrePastCoursePreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        break;
                    case 12:
                        response.Documents = await Task.FromResult(dapper.GetAll<DocumentPreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        break;
                    default:
                        response.AadharData = await Task.FromResult(dapper.Get<AadhaarDetailsPreviewModel>("usp_ScrutinyApplicationDetailsV2", param, commandType: CommandType.StoredProcedure));
                        break;
                }
                return response;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_ScrutinyApplicationDetailsV2", ex);
                return new PreviewModel();
            }
        }
        public async Task<List<Object>> GetParams()
        {
            var response = new List<Object>();
            response.Add(new {Text = "Aadhaar Details",Param = 0 });
            response.Add(new {Text = "Personal Details", Param = 1 });
            response.Add(new {Text = "Caste Details", Param = 2 });
            response.Add(new {Text = "Domicile Details", Param = 3 });
            response.Add(new {Text = "Permanent Address Details", Param = 4 });
            response.Add(new {Text = "Present Address Details", Param = 5 });
            response.Add(new {Text = "Parent Address Details", Param = 6 });
            response.Add(new {Text = "Guardian Address Details", Param = 7 });
            response.Add(new {Text = "Other Details", Param = 8 });
            response.Add(new {Text = "Hostel Preference", Param = 9 });
            response.Add(new {Text = "Current Course", Param = 10 });
            response.Add(new {Text = "Past Course", Param = 11 });
            response.Add(new {Text = "Documents", Param = 12 });
            return response;
        }
        public async Task<bool> ApplicationValidate(long _AppId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_ApplicationId", _AppId, DbType.Int64);
                param.Add("p_HostelId", !string.IsNullOrWhiteSpace(currentUserService.HostelId) ? currentUserService.HostelId : null, DbType.Int32);
                param.Add("p_District", !string.IsNullOrWhiteSpace(currentUserService.DistrictId) ? currentUserService.DistrictId : null, DbType.Int32);
                param.Add("Result", dbType: DbType.Byte, direction: ParameterDirection.Output);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                await Task.FromResult(dapper.Execute("usp_ValidateApplication", param, CommandType.StoredProcedure));
                bool result = param.Get<byte>("Result") == 1;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_ValidateApplication", ex);
                return false;
            }
        }
        public async Task<bool> ApplicationFileValidate(long _AppId,string FileId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_ApplicationId", _AppId, DbType.Int64);
                param.Add("p_FileId", FileId, DbType.String);                
                param.Add("Result", dbType: DbType.Byte, direction: ParameterDirection.Output);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                await Task.FromResult(dapper.Execute("usp_ValidateApplicationFile", param, CommandType.StoredProcedure));
                bool result = param.Get<byte>("Result") == 1;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_ValidateApplicationFile", ex);
                return false;
            }
        }
    }
}
