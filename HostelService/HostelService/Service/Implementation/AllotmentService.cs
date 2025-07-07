using Dapper;
using Helper;
using Model;
using Mysqlx.Session;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation
{
    public class AllotmentService : IAllotmentService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;
        public AllotmentService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService,IErrorLogger errorLogger)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }
        public async Task<TableResponseModel<AllotWaitApplicationModel>> GetListAsync(SearchAllotmentStatusModel model)
        {
            try
            {
                var response = new TableResponseModel<AllotWaitApplicationModel>();
                var param = new DynamicParameters();
                param.Add("p_CourseType", model.CourseType, DbType.Int32);
                param.Add("p_CasteCategory", model.CasteCategory, DbType.Int32);
                param.Add("p_Caste", model.CasteId, DbType.Int32);
                param.Add("p_Status", model.Status, DbType.Int32);
                param.Add("p_StartIndex", model.PageIndex, DbType.Int32);
                param.Add("p_PageSize", model.PageSize, DbType.Int32);
                var result = new List<IEnumerable<dynamic>>();
                if (!string.IsNullOrWhiteSpace(currentUserService.HostelId))
                {
                    param.Add("p_Hostelid", !string.IsNullOrWhiteSpace(currentUserService.HostelId) ? currentUserService.HostelId : null, DbType.Int64);
                    var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                    result = await dapper.MultiResult("usp_AllotmentWaitingApplicationsListWarden", param, commandType: CommandType.StoredProcedure);
                }
                else
                {
                    param.Add("p_District", !string.IsNullOrWhiteSpace(currentUserService.DistrictId) ? currentUserService.DistrictId : null, DbType.Int32);
                    var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                    result = await dapper.MultiResult("usp_AllotmentWaitingApplicationsListDept", param, commandType: CommandType.StoredProcedure);
                }
                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[0].ToList());
                        response.List = JsonConvert.DeserializeObject<List<AllotWaitApplicationModel>>(data);
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
                    await errorLogger.Log("usp_AllotmentWaitingApplicationsListWarden", ex);
                else
                    await errorLogger.Log("usp_AllotmentWaitingApplicationsListDept", ex);

                return new TableResponseModel<AllotWaitApplicationModel> { List = new List<AllotWaitApplicationModel>(), PageIndex = model.PageIndex, PageSize = model.PageSize, Total = 0 };
            }
        }
        public async Task<TableResponseModel<AllotmentModel>> GetAllListAsync(SearchAllotmentStatusModel model)
        {
            try
            {
                var response = new TableResponseModel<AllotmentModel>();
                var param = new DynamicParameters();
                param.Add("p_CourseType", model.CourseType, DbType.Int32);
                param.Add("p_CasteCategory", model.CasteCategory, DbType.Int32);
                param.Add("p_Caste", model.CasteId, DbType.Int32);
                param.Add("p_Status", model.Status, DbType.Int32);
                param.Add("p_Quota", model.QuotaId, DbType.Int32);
                param.Add("p_StartIndex", model.PageIndex, DbType.Int32);
                param.Add("p_PageSize", model.PageSize, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = new List<IEnumerable<dynamic>>();
                if (!string.IsNullOrWhiteSpace(currentUserService.HostelId))
                {
                    param.Add("p_Hostelid", !string.IsNullOrWhiteSpace(currentUserService.HostelId) ? currentUserService.HostelId : null, DbType.Int64);
                    result = await dapper.MultiResult("usp_AllotmentListWarden", param, commandType: CommandType.StoredProcedure);
                }
                else
                {
                    if (model.HostelId > 0)
                        param.Add("p_Hostelid", model.HostelId, DbType.Int64);
                    param.Add("p_District", !string.IsNullOrWhiteSpace(currentUserService.DistrictId) ? currentUserService.DistrictId : null, DbType.Int32);
                    result = await dapper.MultiResult("usp_AllotmentListDept", param, commandType: CommandType.StoredProcedure);
                }
                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[0].ToList());
                        response.List = JsonConvert.DeserializeObject<List<AllotmentModel>>(data);
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
                    await errorLogger.Log("usp_AllotmentListWarden", ex);
                else
                    await errorLogger.Log("usp_AllotmentListDept", ex);

                return new TableResponseModel<AllotmentModel>() { List = new List<AllotmentModel>(), PageIndex = 1, PageSize = 10, Total = 0 };
            }
        }
        public async Task<List<AllotmentCountModel>> GetCourseWiseList(int? HostelId, int? courseID)
        {
            try
            {
                var param = new DynamicParameters();
                if (HostelId > 0)
                    param.Add("hostelID", HostelId, DbType.Int32);
                if (!string.IsNullOrWhiteSpace(currentUserService.HostelId))
                    param.Add("hostelID", currentUserService.HostelId, DbType.Int32);
                param.Add("courseID", courseID, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var response = await Task.FromResult(dapper.GetAll<AllotmentCountModel>("usp_GetHostelAllocationCoursewise", param, CommandType.StoredProcedure));
                return response;
            }
            catch (Exception ex)
            {
                await errorLogger.Log("usp_GetHostelAllocationCoursewise", ex);
                return new List<AllotmentCountModel>();
            }
        }
        public async Task<List<AllotmentCountModel>> GetCategoryWiseList(int? HostelId, int? courseID)
        {
            try
            {
                var param = new DynamicParameters();
                if (HostelId > 0)
                    param.Add("hostelID", HostelId, DbType.Int32);
                if (!string.IsNullOrWhiteSpace(currentUserService.HostelId))
                    param.Add("hostelID", currentUserService.HostelId, DbType.Int32);
                param.Add("courseID", courseID, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var response = await Task.FromResult(dapper.GetAll<AllotmentCountModel>("usp_GetHostelAllocationCategorywise", param, CommandType.StoredProcedure));
                return response;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetHostelAllocationCategorywise", ex);
                return new List<AllotmentCountModel>();
            }
        }
        public async Task<List<CastwiseCountModel>> GetCasteWiseList(int? HostelId, int? courseID, int? casteID)
        {
            try
            {
                var param = new DynamicParameters();
                if (HostelId > 0)
                    param.Add("hostelID", HostelId, DbType.Int32);
                if (!string.IsNullOrWhiteSpace(currentUserService.HostelId))
                    param.Add("hostelID", currentUserService.HostelId, DbType.Int32);
                param.Add("courseID", courseID, DbType.Int32);
                param.Add("casteID", casteID, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var response = await Task.FromResult(dapper.GetAll<CastwiseCountModel>("usp_GetHostelAllocationCastwise", param, CommandType.StoredProcedure));
                return response;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetHostelAllocationCastwise", ex);
                return new List<CastwiseCountModel>();
            }
        }

        public async Task<List<SelectListModel>> GetHostelList()
        {
            try
            {
               
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var param = new DynamicParameters();
                param.Add("p_district", currentUserService.DistrictId, DbType.Int16);
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetHostelAsperDistrict", param, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return new List<SelectListModel>();
            }
        }

        public async Task<List<SelectListModel>> GetHostelListDistrictWiseDeleteSjsaAadhaar(int? DistrictId)
        {
            try
            {

                var dapper = dapperresolverInstance.Resolve("1");
                var param = new DynamicParameters();
                param.Add("p_district", DistrictId, DbType.Int16);
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetHostelAsperDistrict", param, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return new List<SelectListModel>();
            }
        }

    }
}


