using Dapper;
using Helper;
using Model;
using Newtonsoft.Json;
using Repository.Interface;
using Service.Interface;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service.Implementation
{
    public class OfflineRegistrationService : IOfflineRegistrationService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;

        public OfflineRegistrationService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService, IErrorLogger errorLogger)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }
        public bool IsRecordValidate(OfflineAadhharList model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model?.UIDNo))
                {
                    return false;
                }
                if (model?.UIDNo?.Length != 12)
                {
                    return false;
                }
                if (int.Parse(model?.UIDNo.Substring(0, 1)) < 2)
                {
                    return false;
                }
                if (!AadhaarVerifyAlgorithm.ValidateAadhaar(model?.UIDNo))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return false;
            }
        }
        public async Task<List<OfflineRegistrationModel>> GetListAsync(int? HostelId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", HostelId.HasValue ? HostelId : currentUserService.HostelId, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var data = await Task.FromResult(dapper.GetAll<OfflineRegistrationModel>("usp_GetOfflineApplicant", param, commandType: CommandType.StoredProcedure));
                return data;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetOfflineApplicant", ex);
                return new List<OfflineRegistrationModel>();
            }
        }
        public async Task<List<GetOfflineAadhharList>> GetListOffline(int? HostelId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", HostelId.HasValue ? HostelId : currentUserService.HostelId, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var data = await Task.FromResult(dapper.GetAll<GetOfflineAadhharList>("usp_GetOfflineApplicant", param, commandType: CommandType.StoredProcedure));
                return data;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetOfflineApplicant", ex);
                return new List<GetOfflineAadhharList>();
            }
        }
        public async Task Registration(string JsonData)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", currentUserService.HostelId, DbType.Int32);
                param.Add("p_UserId", currentUserService.UserId, DbType.String);
                param.Add("@jsonArray", JsonData, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                await Task.FromResult(dapper.Insert<int>("usp_UpdateOfflineApplicant", param, commandType: CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_UpdateOfflineApplicant", ex);
            }
        }
        public async Task ExistingAadharRegistration(string JsonData, string FilePath)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", currentUserService.HostelId, DbType.Int32);
                param.Add("p_UserId", currentUserService.UserId, DbType.String);
                param.Add("@jsonArray", JsonData, DbType.String);
                param.Add("p_FilePath", FilePath, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);

                await Task.FromResult(dapper.Insert<int>("usp_UpdateExistingApplicant", param, commandType: CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_UpdateExistingApplicant", ex);
            }
        }
        public async Task<List<OfflineAadhharList>> AdminQuery(string query, string caller)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", caller, DbType.String);
                param.Add("p_Query", query, DbType.String);
                param.Add("reportID", 0, DbType.Int32);
                param.Add("p_from", caller, DbType.String);
                param.Add("p_reportName", caller, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var data = await Task.FromResult(dapper.GetAll<OfflineAadhharList>("Admin_Utility", param, commandType: CommandType.StoredProcedure));
                return data;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetOfflineApplicant", ex);
                return new List<OfflineAadhharList>();
            }
        }
        public async Task<int> UpdateAdminQuery(string query, string caller)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", caller, DbType.String);
                param.Add("p_reportName", caller, DbType.String);
                param.Add("reportID", 0, DbType.Int32);
                param.Add("p_from", caller, DbType.String);
                param.Add("p_Query", query, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var data = await Task.FromResult(dapper.Update<int>("Admin_Utility", param, commandType: CommandType.StoredProcedure));
                return data;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetOfflineApplicant", ex);
                return 0;
            }
        }
        public async Task<DirectVacancyCount?> DirectVacancyExistingValue()
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", currentUserService.HostelId, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var data = await Task.FromResult(dapper.Insert<DirectVacancyCount>("usp_GetDirectVacancyExistingValue", param, commandType: CommandType.StoredProcedure));
                return data;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetDirectVacancyExistingValue", ex);
                return null;
            }
        }

        public async Task<List<GetOfflineAadhharList>> GetListOfflineExisting(int? HostelId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", HostelId.HasValue ? HostelId : currentUserService.HostelId, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var data = await Task.FromResult(dapper.GetAll<GetOfflineAadhharList>("usp_GetExistingApplicant", param, commandType: CommandType.StoredProcedure));
                return data;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetExistingApplicant", ex);
                return new List<GetOfflineAadhharList>();
            }
        }

        public async Task<TableResponseModel<ExistingAadharData>> GetListAsync(SearchExistingAadharDataModel model)
        {
            try
            {
                var response = new TableResponseModel<ExistingAadharData>();
                var param = new DynamicParameters();

                param.Add("p_StartIndex", model.PageIndex, DbType.Int32);
                param.Add("p_PageSize", model.PageSize, DbType.Int32);

                var result = new List<IEnumerable<dynamic>>();
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);

                if (!string.IsNullOrWhiteSpace(currentUserService.HostelId))
                    param.Add("p_Hostelid", currentUserService.HostelId, DbType.Int32);
                else
                    param.Add("p_Hostelid", model.HostelId, DbType.Int32);

                result = await dapper.MultiResult("usp_GetUploadedExisingData", param, commandType: CommandType.StoredProcedure);

                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        List<ExistingAadharData> aadharData = new List<ExistingAadharData>();
                        foreach (var item in result[0].ToList())
                        {
                            ExistingAadharData objmodel1 = new ExistingAadharData();
                            objmodel1.Id = item.Id;
                            objmodel1.EncryptedAadhar = item.EncryptedAadhar;

                            //objmodel1.Aadhar = QueryStringEncryptDecrypt.DecryptQueryString(item.EncryptedAadhar.ToString());
                            var qs = QueryStringEncryptDecrypt.DecryptAadhaarNo(item.EncryptedAadhar.ToString());
                            string BankAccountNo = qs["AadhaarNo"];
                            objmodel1.Aadhar = BankAccountNo;

                            objmodel1.ApplicantName = item.ApplicantName;
                            objmodel1.HostelName = item.HostelName;
                            objmodel1.coursetype = item.coursetype;
                            objmodel1.ExcelAdmissionYear = item.ExcelAdmissionYear;
                            objmodel1.ApplicationType = item.ApplicationType;
                            objmodel1.ServiceType = item.ServiceType;
                            objmodel1.CreatedOn = item.CreatedOn.ToString();
                            objmodel1.ApplicationNo = item.ApplicationNo;
                            aadharData.Add(objmodel1);
                        }

                        var data = JsonConvert.SerializeObject(aadharData);
                        response.List = JsonConvert.DeserializeObject<List<ExistingAadharData>>(data);
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
                    await errorLogger.Log("usp_GetUploadedExisingData", ex);
                else
                    await errorLogger.Log("usp_GetUploadedExisingData", ex);
                return new TableResponseModel<ExistingAadharData>() { List = new List<ExistingAadharData>(), PageIndex = 1, PageSize = 10, Total = 0 };
            }
        }

        public async Task<TableResponseModel<ExistingAadharData>> GetListAsync(SearchDeleteExistingAadharDataModel model)
        {
            try
            {
                var response = new TableResponseModel<ExistingAadharData>();
                var param = new DynamicParameters();

                param.Add("p_StartIndex", model.PageIndex, DbType.Int32);
                param.Add("p_PageSize", model.PageSize, DbType.Int32);
                param.Add("p_UserId", currentUserService.UserId, DbType.String);

                var result = new List<IEnumerable<dynamic>>();
                //var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);

                if (!string.IsNullOrWhiteSpace(currentUserService.HostelId))
                    param.Add("p_Hostelid", currentUserService.HostelId, DbType.Int32);
                else
                    param.Add("p_Hostelid", model.HostelId, DbType.Int32);

                result = await dapper.MultiResult("usp_GetDeletedExisingData", param, commandType: CommandType.StoredProcedure);

                if (result.Count > 0)
                {
                    if (result[0] != null)
                    {
                        List<ExistingAadharData> aadharData = new List<ExistingAadharData>();
                        foreach (var item in result[0].ToList())
                        {
                            ExistingAadharData objmodel1 = new ExistingAadharData();
                            objmodel1.Id = item.Id;
                            objmodel1.EncryptedAadhar = item.EncryptedAadhar;

                            //objmodel1.Aadhar = QueryStringEncryptDecrypt.DecryptQueryString(item.EncryptedAadhar.ToString());
                            var qs = QueryStringEncryptDecrypt.DecryptAadhaarNo(item.EncryptedAadhar.ToString());
                            string BankAccountNo = qs["AadhaarNo"];
                            objmodel1.Aadhar = BankAccountNo;

                            objmodel1.ApplicantName = item.ApplicantName;
                            objmodel1.HostelName = item.HostelName;
                            objmodel1.coursetype = item.coursetype;
                            objmodel1.ExcelAdmissionYear = item.ExcelAdmissionYear;
                            objmodel1.ApplicationType = item.ApplicationType;
                            objmodel1.ServiceType = item.ServiceType;
                            objmodel1.CreatedOn = item.CreatedOn.ToString();
                            objmodel1.ApplicationNo = item.ApplicationNo;
                            aadharData.Add(objmodel1);
                        }

                        var data = JsonConvert.SerializeObject(aadharData);
                        response.List = JsonConvert.DeserializeObject<List<ExistingAadharData>>(data);
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
                    await errorLogger.Log("usp_GetUploadedExisingData", ex);
                else
                    await errorLogger.Log("usp_GetUploadedExisingData", ex);
                return new TableResponseModel<ExistingAadharData>() { List = new List<ExistingAadharData>(), PageIndex = 1, PageSize = 10, Total = 0 };
            }
        }

        public async Task<string> DeleteExistingAadharRegistration(string HostelId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", HostelId, DbType.String);
                param.Add("p_UserId", currentUserService.UserId, DbType.String);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);

                var data = await Task.FromResult(dapper.Insert<string>("usp_DeleteExistingApplicantAadhaar", param, commandType: CommandType.StoredProcedure));
                return data;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_DeleteExistingApplicantAadhaar", ex);
                return "Error";
            }
        }

        public bool IsAadhaarValidate(string UIDNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(UIDNo))
                {
                    return false;
                }
                if (UIDNo?.Length != 12)
                {
                    return false;
                }
                if (int.Parse(UIDNo.Substring(0, 1)) < 2)
                {
                    return false;
                }
                if (!AadhaarVerifyAlgorithm.ValidateAadhaar(UIDNo))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return false;
            }
        }

        public async Task<string> DeleteExistingAadhaarId(int? Id, string? HostelId)
        {
            try
            {
                var param = new DynamicParameters();
                if (!string.IsNullOrWhiteSpace(currentUserService.HostelId))
                    param.Add("p_Hostelid", currentUserService.HostelId, DbType.Int32);
                else
                    param.Add("p_Hostelid", HostelId, DbType.Int32);

                param.Add("p_UserId", currentUserService.UserId, DbType.String);
                param.Add("p_Id", Id, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var data = await Task.FromResult(dapper.Update<string>("usp_DeleteExisingData", param, commandType: CommandType.StoredProcedure));
                return data;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetOfflineApplicant", ex);
                return "Error";
            }
        }
    }
}