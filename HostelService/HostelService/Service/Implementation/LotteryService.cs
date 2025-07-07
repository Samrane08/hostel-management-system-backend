using Dapper;
using Helper;
using Model;
using Model.LotteryModel;
using Newtonsoft.Json;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation
{
    public class LotteryService : ILotteryService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;

        public LotteryService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService, IErrorLogger errorLogger)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }
        public async Task<LotteryGlobalModel> GetData(int? DistrictId, int? CourseId, int? Hostelid)
        {
            try
            {
                var result = new List<IEnumerable<dynamic>>();
                var response = new LotteryGlobalModel();

                var param = new DynamicParameters();
                param.Add("p_District", DistrictId, DbType.Int32);
                param.Add("p_CourseId", CourseId, DbType.Int32);
                param.Add("p_HostelId", Hostelid, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                result = await dapper.MultiResult("usp_GetLotteryData", param, commandType: CommandType.StoredProcedure);
                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[0].ToList());
                        response.CourseList = JsonConvert.DeserializeObject<List<CourseTypeModel>>(data);
                    }
                    if (result[1] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[1].ToList());
                        response.ApplicantList = JsonConvert.DeserializeObject<List<ApplicantModel>>(data);
                    }
                    if (result[2] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[2].ToList());
                        response.PriorityList = JsonConvert.DeserializeObject<List<ProrityModel>>(data);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                throw;
            }
        }
        public async Task<LotteryGlobalModel> GenerateLottery(int? DistrictId, int? CourseId, int? HostelId)
        {
            try
            {
                var result = new List<IEnumerable<dynamic>>();
                var response = new LotteryGlobalModel();

                var param = new DynamicParameters();
                param.Add("p_District", DistrictId, DbType.Int32);
                param.Add("p_CourseId", CourseId, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                result = await dapper.MultiResult("usp_GetLotteryData", param, commandType: CommandType.StoredProcedure);
                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[0].ToList());
                        response.CourseList = JsonConvert.DeserializeObject<List<CourseTypeModel>>(data);
                    }
                    if (result[1] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[1].ToList());
                        response.ApplicantList = JsonConvert.DeserializeObject<List<ApplicantModel>>(data);
                    }
                    if (result[2] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[2].ToList());
                        response.PriorityList = JsonConvert.DeserializeObject<List<ProrityModel>>(data);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                throw;
            }
        }

        public async Task<List<HostelPreferenceModel>> GetApplicantPreference(long Userid)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", Userid, DbType.Int64);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<HostelPreferenceModel>("usp_Lottery_GetPreferences", param, commandType: CommandType.StoredProcedure));
                return result.ToList();
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));

                return new List<HostelPreferenceModel>();
            }
        }
        public async Task<HostelVacancyModel> GetHostelVacancy(int CourseId, int HostelId, int Priority)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_CourseId", CourseId, DbType.Int32);
                param.Add("p_HostelId", HostelId, DbType.Int32);
                param.Add("p_Priority", Priority, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Get<HostelVacancyModel>("usp_Lottery_GetVacancy", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return new HostelVacancyModel();
            }
        }
        public async Task<int> GetMeritGenerationStatus(int? CourseId )
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_DistrictId", currentUserService.DistrictId, DbType.Int32);
                param.Add("p_CourseId", CourseId, DbType.Int32 );
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId );
                var result = await Task.FromResult(dapper.Get<int>("usp_GetMerit_Generation_Status", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex)) ;
                return 0;
            }
        }

        public async Task<MeritGlobalModel> MeritGenerationReport(int? CourseId)
        {
            try
            {
                var response = new MeritGlobalModel();
                var param = new DynamicParameters();
                param.Add("p_District", currentUserService.DistrictId, DbType.Int32);
                param.Add("p_CourseId", CourseId, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await dapper.MultiResult("usp_GetDistrictWiseMeritReport", param, commandType: CommandType.StoredProcedure);
               
                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[0].ToList());
                        response.CourseList = JsonConvert.DeserializeObject<List<MeritPreference>>(data);
                    }
                    //if (result[1] != null)
                    //{
                    //    var data = JsonConvert.SerializeObject(result[1].ToList());
                    //    response.ApplicantList = JsonConvert.DeserializeObject<List<ApplicantModel>>(data);
                    //}
                    //if (result[2] != null)
                    //{
                    //    var data = JsonConvert.SerializeObject(result[2].ToList());
                    //    response.PriorityList = JsonConvert.DeserializeObject<List<ProrityModel>>(data);
                    //}
                }
                return response;
                //foreach (var row in result[0])
                //{
                //var dict = (IDictionary<string, object>)row;
                //dictionaryList.Add(new Dictionary<string, object>(dict));
                //}
                //var result = await Task.FromResult(dapper.Get<int>("usp_GetDistrictWiseMeritReport", param, commandType: CommandType.StoredProcedure));
                
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                throw;
            }
        }


        public async Task UpdateAllotment(long UserId, int HostelId, string ApplicationNo, int CourseType, int CasteCategoryId, int CasteId, int Priority)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", UserId, DbType.Int64);
                param.Add("p_HostelId", HostelId, DbType.Int32);
                param.Add("p_ApplicationNo", ApplicationNo, DbType.String);
                param.Add("p_CourseType", CourseType, DbType.Int32);
                param.Add("p_CasteCategoryId", CasteCategoryId, DbType.Int32);
                param.Add("p_CasteId", CasteId, DbType.Int32);
                param.Add("p_Priority", Priority, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                await Task.FromResult(dapper.Execute("usp_Lottery_AllotmentUpdate", param, commandType: CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));

            }
        }
        public async Task AllotmentGenerate(GenerateLotteryModel model)
        {
            try
            {
                //var param = new DynamicParameters();               
                //param.Add("p_DistrictId",model.DistrictId == null ? currentUserService.DistrictId :model.DistrictId, DbType.Int32);                
                //param.Add("p_HostelId", model.HostelId, DbType.Int32);                
                //param.Add("p_CourseId", model.CourseId, DbType.Int32);              
                //param.Add("p_SkipValidation",0, DbType.Int16);
                // for (int i = 0; i < 2; i++)
                // {

                for (int j = 1; j <= 11; j++)
                {

                    var param = new DynamicParameters();
                    param.Add("p_DistrictId", model.DistrictId == null ? currentUserService.DistrictId : model.DistrictId, DbType.Int32);
                    param.Add("p_HostelId", null, DbType.Int32);
                    param.Add("p_CourseId", model.CourseId, DbType.Int32);

                    var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                    //param.Add("p_SkipValidation", 0, DbType.Int16);
                    switch (j)
                    {
                        case 1:
                            param.Add("p_Priority", 1, DbType.Int32);
                            await Task.FromResult(dapper.Execute("usp_Seat_Allocation_For_Disable", param, commandType: CommandType.StoredProcedure));
                            break;
                        case 2:
                            param.Add("p_Priority", 2, DbType.Int32);
                            await Task.FromResult(dapper.Execute("usp_Seat_Allocation_For_Orphan", param, commandType: CommandType.StoredProcedure));
                            break;
                        case 3:
                            param.Add("p_Priority", 3, DbType.Int32);
                            await Task.FromResult(dapper.Execute("usp_Seat_Allocation_For_MangMatangCaste", param, commandType: CommandType.StoredProcedure));
                            break;
                        case 4:
                            param.Add("p_Priority", 4, DbType.Int32);
                            await Task.FromResult(dapper.Execute("usp_Seat_Allocation_For_MehtarCaste", param, commandType: CommandType.StoredProcedure));
                            break;
                        case 5:
                            param.Add("p_Priority", 5, DbType.Int32);
                            await Task.FromResult(dapper.Execute("usp_Seat_Allocation_For_OtherCaste", param, commandType: CommandType.StoredProcedure));
                            break;
                        case 6:
                            param.Add("p_Priority", 6, DbType.Int32);
                            await Task.FromResult(dapper.Execute("usp_Seat_Allocation_For_SBCCaste", param, commandType: CommandType.StoredProcedure));
                            break;
                        case 7:
                            param.Add("p_Priority", 7, DbType.Int32);
                            await Task.FromResult(dapper.Execute("usp_Seat_Allocation_For_EBCOBCCaste", param, commandType: CommandType.StoredProcedure));
                            break;
                        case 8:
                            param.Add("p_Priority", 8, DbType.Int32);
                            await Task.FromResult(dapper.Execute("usp_Seat_Allocation_For_VJNTCaste", param, commandType: CommandType.StoredProcedure));
                            break;
                        case 9:
                            param.Add("p_Priority", 9, DbType.Int32);
                            await Task.FromResult(dapper.Execute("usp_Seat_Allocation_For_STCaste", param, commandType: CommandType.StoredProcedure));
                            break;
                        case 10:
                            param.Add("p_Priority", 3, DbType.Int32);
                            await Task.FromResult(dapper.Execute("usp_Seat_Allocation_For_MangMatangCaste_Other", param, commandType: CommandType.StoredProcedure));
                            break;
                        case 11:
                            param.Add("p_Priority", 4, DbType.Int32);
                            await Task.FromResult(dapper.Execute("usp_Seat_Allocation_For_MehtarCaste_Other", param, commandType: CommandType.StoredProcedure));
                            break;
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_Lottery_Seat_Allocation", ex);
            }
        }



        public async Task GenerateGeneralMeritList(GenerateLotteryModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_District",  currentUserService.DistrictId , DbType.Int32);
                param.Add("p_CourseId", model.CourseId, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                await Task.FromResult(dapper.Execute("usp_Generate_Merit_list", param, commandType: CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));

            }
        }

        public async Task<TableResponseModel<MeritResponseModel>> GetMeritApproveList(MeritApproveModel model)
        {
            try
            {
                var response = new TableResponseModel<MeritResponseModel>();
                var result = new List<IEnumerable<dynamic>>();
                var param = new DynamicParameters();

                param.Add("p_District", currentUserService.DistrictId, DbType.Int32);
                param.Add("p_HostelId", model.HostelId, DbType.Int32);
                param.Add("p_CourseId", model.CourseType, DbType.Int32);
                param.Add("p_PageIndex", model.PageIndex, DbType.Int32);
                param.Add("p_PageSize", model.PageSize, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                result = await dapper.MultiResult("usp_getApplicationsDetails", param, commandType: CommandType.StoredProcedure);
                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[0].ToList());
                        response.List = JsonConvert.DeserializeObject<List<MeritResponseModel>>(data);
                    }
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
                return response;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_getApplicationsDetails", ex);
                return new TableResponseModel<MeritResponseModel>() { List = new List<MeritResponseModel>(), PageIndex = 1, PageSize = 10, Total = 0 };
            }
        }

        public async Task<string> SaveMeritApproveList(List<MeritResponseModel> models)
        {
            try
            {
                var jsonArray = JsonConvert.SerializeObject(models.Select(x => new { ApplicationNo = x.ApplicationNo }).ToList());
                var myParams = new DynamicParameters();
                myParams.Add("p_District", currentUserService.DistrictId, DbType.Int32);
                myParams.Add("@jsonArray", jsonArray, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Insert<string>("usp_UpdateLotteryApplicationsStatus", myParams, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_UpdateLotteryApplicationsStatus", ex);
                throw;
            }
        }

        public async Task<TableResponseModel<MeritResponseModel>> GetMeritAsstRejectList(MeritRejectModel model)
        {
            try
            {
                var response = new TableResponseModel<MeritResponseModel>();
                var result = new List<IEnumerable<dynamic>>();
                var param = new DynamicParameters();

                param.Add("p_District", currentUserService.DistrictId, DbType.Int32);
                param.Add("p_CourseId", model.CourseType, DbType.Int32);
                param.Add("p_PageIndex", model.PageIndex, DbType.Int32);
                param.Add("p_PageSize", model.PageSize, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                result = await dapper.MultiResult("usp_getMeritAsstRejectDetails", param, commandType: CommandType.StoredProcedure);
                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[0].ToList());
                        response.List = JsonConvert.DeserializeObject<List<MeritResponseModel>>(data);
                    }
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
                return response;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_getMeritAsstRejectDetails", ex);
                return new TableResponseModel<MeritResponseModel>() { List = new List<MeritResponseModel>(), PageIndex = 1, PageSize = 10, Total = 0 };
            }
        }

        public async Task<string> SaveMeritAsstRejectList(List<MeritResponseModel> models)
        {
            try
            {
                var jsonArray = JsonConvert.SerializeObject(models.Select(x => new { ApplicationNo = x.ApplicationNo }).ToList());
                var myParams = new DynamicParameters();
                myParams.Add("p_District", currentUserService.DistrictId, DbType.Int32);
                myParams.Add("@jsonArray", jsonArray, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Insert<string>("usp_UpdateLotteryAsstRejectStatus", myParams, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_UpdateLotteryAsstRejectStatus", ex);
                throw;
            }
        }

       
    }
}