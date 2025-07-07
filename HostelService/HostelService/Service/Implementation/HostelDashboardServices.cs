using Dapper;
using Helper;
using Model;
using Newtonsoft.Json;
using Repository.Interface;
using Service.Interface;
using System.Data;
using System.Threading.Channels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service.Implementation
{
    public class HostelDashboardServices : IHostelDashboardService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;

        public HostelDashboardServices(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService,IErrorLogger errorLogger)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }
        public async Task<HostelDashboardModel?> GetHostelDashboard(int? ServiceType, int? Installment,int? academicyear)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", !string.IsNullOrWhiteSpace(currentUserService.HostelId)? currentUserService.HostelId : null, DbType.Int64);
                param.Add("p_DistrictId", !string.IsNullOrWhiteSpace(currentUserService.DistrictId) ? currentUserService.DistrictId : null, DbType.Int32);
                param.Add("p_WorkFlowId", !string.IsNullOrWhiteSpace(currentUserService.WorkFlowId) ? currentUserService.WorkFlowId : null, DbType.Int32);
                param.Add("p_ServiceType", ServiceType == 0 ? null : ServiceType, DbType.Int32);
                param.Add("p_Installment", Installment == 1 ? null : Installment, DbType.Int32);
                param.Add("p_AcademicYear", academicyear, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Get<HostelDashboardModel>("usp_HostelDashboardCountNew", param, commandType: CommandType.StoredProcedure));
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new HostelDashboardModel()
                    {
                        Admitted = 0,
                        Approved = 0,
                        Rejected = 0,
                        Renewals = 0,
                        UnderScrutiny = 0,
                        SentBack = 0,
                        TotalApplication = 0,
                        TotalRegistered = 0,
                        AdmittedFlag = 0,
                        ApprovedFlag = 0,
                        RejectedFlag = 0,
                        RenewalsFlag = 0,
                        UnderScrutinyFlag = 0,
                        SentBackFlag = 0,
                        TotalApplicationFlag = 0,
                        TotalRegisteredFlag = 0,
                        CancelFlag=0,
                        Cancel=0
                    };
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_HostelDashboardCount", ex);
                return new HostelDashboardModel()
                {
                    Admitted = 0,
                    Approved = 0,
                    Rejected = 0,
                    Renewals = 0,
                    UnderScrutiny = 0,
                    SentBack = 0,
                    TotalApplication = 0,
                    TotalRegistered = 0,
                    AdmittedFlag = 0,
                    ApprovedFlag = 0,
                    RejectedFlag = 0,
                    RenewalsFlag = 0,
                    UnderScrutinyFlag = 0,
                    SentBackFlag = 0,
                    TotalApplicationFlag = 0,
                    TotalRegisteredFlag = 0,
                    CancelFlag = 0,
                    Cancel = 0
                };               
            }
        }
        public async Task<HostelDashboardModel?> GetBADashboardApplication(int? division, int? district, int? applicationServiceType)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_DivisionId", division != null ? division : null, DbType.Int32);
                param.Add("p_DistrictId", district != null ? district : null, DbType.Int32);
                param.Add("p_ApplicationServiceType", applicationServiceType != null ? applicationServiceType : null, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Get<HostelDashboardModel>("usp_BADashboardCountApplication", param, commandType: CommandType.StoredProcedure));
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new HostelDashboardModel()
                    {
                        Admitted = 0,
                        Approved = 0,
                        Rejected = 0,
                        Renewals = 0,
                        UnderScrutiny = 0,
                        SentBack = 0,
                        TotalApplication = 0,
                        TotalRegistered = 0,
                        AdmittedFlag = 0,
                        ApprovedFlag = 0,
                        RejectedFlag = 0,
                        RenewalsFlag = 0,
                        UnderScrutinyFlag = 0,
                        SentBackFlag = 0,
                        TotalApplicationFlag = 0,
                        TotalRegisteredFlag = 0
                    };
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_HostelDashboardCount", ex);
                return new HostelDashboardModel()
                {
                    Admitted = 0,
                    Approved = 0,
                    Rejected = 0,
                    Renewals = 0,
                    UnderScrutiny = 0,
                    SentBack = 0,
                    TotalApplication = 0,
                    TotalRegistered = 0,
                    AdmittedFlag = 0,
                    ApprovedFlag = 0,
                    RejectedFlag = 0,
                    RenewalsFlag = 0,
                    UnderScrutinyFlag = 0,
                    SentBackFlag = 0,
                    TotalApplicationFlag = 0,
                    TotalRegisteredFlag = 0
                };
            }
        }
        public async Task<List<SelectListModel>> GetBATileCourseTypeDetailsCount(ApplicationServiceType model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_Division", model.division != null ? model.division : null, DbType.Int32);
                param.Add("p_District", model.district != null ? model.district : null, DbType.Int32);
                param.Add("p_ServiceType", model.serviceType != null ? model.serviceType : null, DbType.Int32);
                param.Add("p_Flag", model.flag, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_BADashboardTileDetailCount", param, commandType: CommandType.StoredProcedure));
                if (result != null)
                {
                    return result;
                }
                else
                {
                   return new List<SelectListModel>();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_BADashboardTileDetailCount", ex);
                return new List<SelectListModel>();
            }
        }

        public async Task<List<SelectListModel>> GetApplicationFilters()
        {
            try
            {
                
                var param = new DynamicParameters();
                if(string.IsNullOrEmpty(currentUserService.WorkFlowId) || currentUserService.WorkFlowId == "1")
                    param.Add("p_WorkFlowId", 1, DbType.Int32);
                else
                   param.Add("p_WorkFlowId", currentUserService.WorkFlowId, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetStatusNew", param, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetStatus", ex);
                return new List<SelectListModel>();
            }
        }
        //public async Task<List<SearchApplicationReponseModel>> GetHostelsApplications(string flag)
        //{
        //    try
        //    {
        //        var param = new DynamicParameters();
        //        param.Add("p_UserId", currentUserService.UserId, DbType.String);
        //        param.Add("p_flag", flag, DbType.String);
        //        var result = await Task.FromResult(dapper.GetAll<SearchApplicationReponseModel>("usp_GetHostelsApplications", param, commandType: CommandType.StoredProcedure));
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        await errorLogger.Log("usp_GetHostelsApplications", ex);
        //        return null;
        //    }
        //}

        // 


        // scrutiny level
        /*  public async Task<List<SelectListModel>> GetscrutinylevelFilters(int? IsServiceType)
          {
              try
              {

                  var myparam = new DynamicParameters();
                  if (IsServiceType != null)
                      myparam.Add("p_IsServiceType", Convert.ToInt32(IsServiceType), DbType.Int16);
                  else
                      myparam.Add("p_IsServiceType", 0, DbType.Int16);
                  var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetScrutinyList", myparam, commandType: System.Data.CommandType.StoredProcedure));
                  return (result);

              }
              catch (Exception ex)
              {
                  ExceptionLogging.LogException(Convert.ToString(ex));
                //  await errorLogger.Log("usp_GetStatus", ex);
                  return new List<SelectListModel>();
              }
          }*/

        public async Task<List<SelectListModel>> GetscrutinylevelFilters(int? IsServiceType, int? IsApplicantnewExisting)
        {
            try
            {

                var myparam = new DynamicParameters();
                myparam.Add("p_IsServiceType", IsServiceType != null ? IsServiceType : 0, DbType.Int32);
                myparam.Add("p_IsApplicantnewExisting", IsApplicantnewExisting != null ? IsApplicantnewExisting : 0, DbType.Int32);
                //if (IsServiceType  != null && IsApplicantnewExisting != null)
                //{
                //    myparam.Add("p_IsServiceType", Convert.ToInt32(IsServiceType), DbType.Int16);
                //    myparam.Add("p_IsApplicantnewExisting", Convert.ToInt32(IsApplicantnewExisting), DbType.Int16);

                //}

                //else
                //{
                //    myparam.Add("p_IsServiceType", 0, DbType.Int16);
                //    myparam.Add("p_IsApplicantnewExisting", 1, DbType.Int16);
                //}
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetScrutinyList", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return (result);

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                //  await errorLogger.Log("usp_GetStatus", ex);
                return new List<SelectListModel>();
            }
        }



        public async Task<List<HostelDashboardModel>> GetBASearchFilter(SearchFilter model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_DivisionId", model.division != null ? model.division : null, DbType.Int32);
                param.Add("p_DistrictId", model.district != null ? model.district : null, DbType.Int32);
                param.Add("p_ApplicationServiceType", model.serviceType != null ? model.serviceType : null, DbType.Int32);
                param.Add("p_ApplicationType", model.applicationType != null ? model.applicationType : null, DbType.Int32);
                param.Add("p_CourseId", model.courseType != null ? model.courseType : null, DbType.Int32);
                param.Add("p_ScrutinyLevel", model.scrutinyLevel != null ? model.scrutinyLevel : null, DbType.Int32);
                param.Add("p_HostelId", model.hostelName != null ? model.hostelName : null, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<HostelDashboardModel>("usp_BADashboardCountApplication", param, commandType: CommandType.StoredProcedure));
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new List<HostelDashboardModel>();
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_BADashboardCountApplication", ex);
                return new List<HostelDashboardModel>();
            }
        }
        //for xl columns api
        private List<string> dataList = new List<string>();
        public async Task<object> GetBASearchFilterCountdata(SearchFilter1 model)

        {
            try
            {

             
        var param = new DynamicParameters();
                param.Add("p_DivisionId", model.division != null ? model.division : null, DbType.Int32);
                param.Add("p_DistrictId", model.district != null ? model.district : null, DbType.Int32);
                param.Add("p_ApplicationServiceType", model.serviceType != null ? model.serviceType : null, DbType.Int32);
                param.Add("p_ApplicationType", model.applicationType != null ? model.applicationType : null, DbType.Int32);
                param.Add("p_CourseId", model.courseType != null ? model.courseType : null, DbType.Int32);
                param.Add("p_ScrutinyLevel", model.scrutinyLevel != null ? model.scrutinyLevel : null, DbType.Int32);
                param.Add("p_HostelId", model.hostelName != null ? model.hostelName : null, DbType.Int32);
                param.Add("p_Flag", model.flag, DbType.String);

                if (model.serviceType == 1)
                {
                    if (model.flag == "TotalApplicationFlag")
                    {
                        var response = new TableResponseModel<BASearchedDataresponseModel1>();
                        var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                        //var result = await Task.FromResult(dapper.GetAll<BASearchedDataresponseModel.BASearchedDataresponseModel1>("usp_BADashboardTileDetailCountData", param, commandType: CommandType.StoredProcedure).ToList());
                        var result = await dapper.MultiResult("usp_BADashboardTileDetailCountData", param, commandType: CommandType.StoredProcedure);

                        var resultMain = new List<IEnumerable<dynamic>>();
                        
                        if (result.Count > 0)
                        {
                            //for (int i =0;i< result.Count; i++)
                            //{
                            //        //data = JsonConvert.SerializeObject(result[i].ToList());
                            //    dataList.Add(JsonConvert.SerializeObject(result[i].ToList()));
                            //}
                            if (result[0].Count() > 0) 
                                resultMain.Add(result[0]);

                            if (result[1].Count() > 0)
                                resultMain.Add(result[1]);

                            if (result[2].Count() > 0)
                                resultMain.Add(result[2]);

                            // resultMain.Add(result[0]);
                            // resultMain.Add(result[1]);
                            // resultMain.Add(result[2]);

                            //dataList.Add(JsonConvert.SerializeObject(result));
                            //dataList.Add(JsonConvert.SerializeObject(result[1]));
                            //dataList.Add(JsonConvert.SerializeObject(result[2]));


                            string combinedData = JsonConvert.SerializeObject(resultMain);

                            combinedData = combinedData.Replace("[", "");
                            combinedData = combinedData.Replace("]", "");

                            combinedData = "[" + combinedData + "]";

                            // Deserialize the combined JSON into the response model

                            response.List = JsonConvert.DeserializeObject<List<BASearchedDataresponseModel1>>(combinedData);


                            //response.List = JsonConvert.DeserializeObject<List<BASearchedDataresponseModel.BASearchedDataresponseModel1>>(data);
                            //response.List = JsonConvert.DeserializeObject<List<BASearchedDataresponseModel.BASearchedDataresponseModel1>>(data);
                            //if (result[1] != null)
                            //{
                            //    var data = JsonConvert.SerializeObject(result[1].FirstOrDefault());
                            //    var paginator = JsonConvert.DeserializeObject<PaginationDataModel>(data);
                            //    if (paginator != null)
                            //    {
                            //        response.PageSize = paginator.PageSize;
                            //        response.PageIndex = paginator.PageIndex;
                            //        response.Total = paginator.Total;
                            //    }
                            //}
                        }
                        //foreach (var row in result[0])
                        //{
                        //    var dict = (IDictionary<string, object>)row;
                        //    dictionaryList.Add(new Dictionary<string, object>(dict));
                        //}

                        return response.List;
                    }
                    else
                    {
                        var response = new TableResponseModel<BASearchedDataresponseModel1>();
                        var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                        var result = await Task.FromResult(dapper.GetAll<BASearchedDataresponseModel1>("usp_BADashboardTileDetailCountData", param, commandType: CommandType.StoredProcedure));
                        string combinedData = JsonConvert.SerializeObject(result);

                        combinedData = combinedData.Replace("[", "");
                        combinedData = combinedData.Replace("]", "");

                        combinedData = "[" + combinedData + "]";

                        // Deserialize the combined JSON into the response model

                        response.List = JsonConvert.DeserializeObject<List<BASearchedDataresponseModel1>>(combinedData);
                        return response.List;
                    }
                    
                    
                }
                else if (model.serviceType == 2){

                    if (model.flag == "TotalApplicationFlag")
                    {
                        var response = new TableResponseModel<BASearchedDataresponseModel2>();
                        var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                        //var result = await Task.FromResult(dapper.GetAll<BASearchedDataresponseModel.BASearchedDataresponseModel1>("usp_BADashboardTileDetailCountData", param, commandType: CommandType.StoredProcedure).ToList());
                        var result = await dapper.MultiResult("usp_BADashboardTileDetailCountData", param, commandType: CommandType.StoredProcedure);

                        var resultMain = new List<IEnumerable<dynamic>>();

                        if (result.Count > 0)
                        {


                            if (result[0].Count() > 0)
                                resultMain.Add(result[0]);

                            if (result[1].Count() > 0)
                                resultMain.Add(result[1]);

                            if (result[2].Count() > 0)
                                resultMain.Add(result[2]);

                           // resultMain.Add(result[0]);
                            //resultMain.Add(result[1]);
                          //  resultMain.Add(result[2]);

                            string combinedData = JsonConvert.SerializeObject(resultMain);

                            combinedData = combinedData.Replace("[", "");
                            combinedData = combinedData.Replace("]", "");

                            combinedData = "[" + combinedData + "]";

                            response.List = JsonConvert.DeserializeObject<List<BASearchedDataresponseModel2>>(combinedData);

                        }

                        return response.List;
                    }
                    else
                    {
                        var response = new TableResponseModel<BASearchedDataresponseModel2>();
                        var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                        var result = await Task.FromResult(dapper.GetAll<BASearchedDataresponseModel2>("usp_BADashboardTileDetailCountData", param, commandType: CommandType.StoredProcedure));
                        string combinedData = JsonConvert.SerializeObject(result);

                        combinedData = combinedData.Replace("[", "");
                        combinedData = combinedData.Replace("]", "");

                        combinedData = "[" + combinedData + "]";

                        // Deserialize the combined JSON into the response model

                        response.List = JsonConvert.DeserializeObject<List<BASearchedDataresponseModel2>>(combinedData);
                        return response.List;
                    }


                    //  var response = new TableResponseModel<BASearchedDataresponseModel2>();
                    //  var result = await Task.FromResult(dapper.GetAll<BASearchedDataresponseModel2>("usp_BADashboardTileDetailCountData", param, commandType: CommandType.StoredProcedure));
                    //  string combinedData = JsonConvert.SerializeObject(result);

                    //   combinedData = combinedData.Replace("[", "");
                    //  combinedData = combinedData.Replace("]", "");

                    //  combinedData = "[" + combinedData + "]";

                    // Deserialize the combined JSON into the response model

                    // response.List = JsonConvert.DeserializeObject<List<BASearchedDataresponseModel2>>(combinedData);
                    //  return response.List;

                }

                else
                {
                    var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                    var result = await Task.FromResult(dapper.GetAll<BASearchedDataresponseModel1>("usp_BADashboardTileDetailCountData", param, commandType: CommandType.StoredProcedure).ToList());

                     return result;
                    //return response.List;
                    //return new List<BASearchedDataresponseModel1>();
                }

                //if (result != null)
                //{
                //    return result;
                //}
                //else
                //{
                //    return new List<BASearchedDataresponseModel>();
                //}
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_BADashboardTileDetailCountData", ex);
                return new List<BASearchedDataresponseModel1>();
            }
        }

        // Task<List<BASearchedDataresponseModel>> IHostelDashboardService.GetBASearchFilterCountdata(SearchFilter model)
        // {
        //    throw new NotImplementedException();
        //  }




        public async Task<List<SelectListAttendence>> GetapplicationNumberFilter(int? ayid)
        {
            try
            {

                var myparam = new DynamicParameters();
                //myparam.Add("p_hostelid", hostelid != null ? hostelid : 0, DbType.Int32);
                myparam.Add("p_HostelId", !string.IsNullOrWhiteSpace(currentUserService.HostelId) ? currentUserService.HostelId : null, DbType.Int64);
                myparam.Add("p_ayid", ayid != null ? ayid : 0, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve("2");
                var result = await Task.FromResult(dapper.GetAll<SelectListAttendence>("usp_GetApplicationByWarden", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return (result);

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                //  await errorLogger.Log("usp_GetStatus", ex);
                return new List<SelectListAttendence>();
            }
        }


        public async Task<List<SelectListModel>> GetInstallmentlevelFilters(int? IsServiceType)
        {
            try
            {

                var myparam = new DynamicParameters();
                myparam.Add("p_ServiceType", IsServiceType != null ? IsServiceType : 0, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve("2");
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetInstallmentList", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return (result);

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                //  await errorLogger.Log("usp_GetStatus", ex);
                return new List<SelectListModel>();
            }
        }



    }
}
