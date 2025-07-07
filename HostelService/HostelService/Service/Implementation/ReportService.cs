using Dapper;
using Helper;
using Model;
using Newtonsoft.Json;
using Repository.Implementation;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation
{
    public class ReportService: IReportService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        public ReportService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
        }
        public async Task<TableResponseModel<ApplicationModel>> GetListAsync(ReportFilterModel model)
        {
            try
            {
                var response = new TableResponseModel<ApplicationModel>();
                var param = new DynamicParameters();
                param.Add("p_AcademicYear", model.AcademicYear, DbType.Int32);
                param.Add("p_HostelId", model.HostelId, DbType.Int32);
                param.Add("p_DistrictId", model.DistrictId, DbType.Int32);
                param.Add("p_Caste", model.Caste, DbType.Int32);
                param.Add("p_CasteCategory", model.CasteCategory, DbType.Int32);
                param.Add("p_StartDate", model.StartDate?.ToString("yyyy-MM-dd"), DbType.Date);
                param.Add("p_EndDate", model.EndDate?.ToString("yyyy-MM-dd"), DbType.Date);        
                param.Add("p_StartIndex", model.PageIndex, DbType.Int32);
                param.Add("p_PageSize", model.PageSize, DbType.Int32);
                param.Add("p_IsPrint", model.Print, DbType.Boolean);
                var result = new List<IEnumerable<dynamic>>();
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                result = await dapper.MultiResult("usp_Application_Report", param, commandType: CommandType.StoredProcedure);
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
                return new TableResponseModel<ApplicationModel>() { List = new List<ApplicationModel>(), PageIndex = 0, PageSize = 0, Total = 0 };
            }
        }
        public async Task<List<Dictionary<string, object>>> GetListAsyncForWarden()
        {
            try
            {
                var dictionaryList = new List<Dictionary<string, object>>();
                var param = new DynamicParameters();
                param.Add("p_HostelID", currentUserService.HostelId, DbType.Int32);
                param.Add("p_DistrictID", currentUserService.DistrictId, DbType.Int32);
                param.Add("p_WorkFLowID", currentUserService.WorkFlowId, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await dapper.MultiResult("usp_GetApplicationdataByHostelID", param, commandType: CommandType.StoredProcedure);
              
                if (result.Count > 0)
                {
                    
                    if (result[0] != null)
                    {

                        foreach (var row in result[0])
                        {
                            var dict = (IDictionary<string, object>)row;
                            dictionaryList.Add(new Dictionary<string, object>(dict));
                        }
                      
                    }
                }
                //var result = await Task.FromResult<WardenApplicationModel>(dapper.GetAll("usp_GetApplicationdataByHostelID", param, commandType: CommandType.StoredProcedure));

                return dictionaryList;

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                throw;
            }
        }

        public async Task<List<Dictionary<string, object>>> GetDailyReports(int? reportId)
        {
            try
            {
                int wrkflowId = 0;
                if(!string.IsNullOrEmpty(currentUserService.WorkFlowId))
                    wrkflowId=Convert.ToInt32(currentUserService.WorkFlowId);

               // var dictionaryList = new List<Dictionary<string, object>>();
                var param = new DynamicParameters();
                param.Add("p_ReportID", reportId, DbType.Int32);
                param.Add("p_workflowId", wrkflowId, DbType.Int32);
                param.Add("p_UserIdentity", currentUserService.UserId, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await dapper.GetAllAsDictionaryAsync("usp_Get_Daily_Report", param, commandType: CommandType.StoredProcedure);

                //foreach (var row in result[0])
                //{
                //    var dict = (IDictionary<string, object>)row;
                //    dictionaryList.Add(new Dictionary<string, object>(dict));
                //}

                //if (result != null && reportId is not null)
                //{

                //   foreach (var row in result[0])
                //        {
                //            var dict = (IDictionary<string, object>)row;
                //            dictionaryList.Add(new Dictionary<string, object>(dict));
                //        }
                //}
                //else if(result != null && reportId is null)
                //{

                //    foreach (var row in result[0])
                //    {
                //        var dict = (IDictionary<string, object>)row;
                //        dictionaryList.Add(new Dictionary<string, object>(dict));
                //    }
                //}
                //var result = await Task.FromResult<WardenApplicationModel>(dapper.GetAll("usp_GetApplicationdataByHostelID", param, commandType: CommandType.StoredProcedure));

                return result;

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                throw;
            }
        }
    }
}
