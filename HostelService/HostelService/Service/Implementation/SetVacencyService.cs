using Dapper;
using Helper;
using Model;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation
{
    public class SetVacencyService : ISetVacencyService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        public SetVacencyService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
        }

        public async Task<List<FetchVacancyQuotaTypeResponse>?> FetchQuotaTypeAsync()
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", currentUserService.HostelId, DbType.Int64);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<FetchVacancyQuotaTypeResponse>("usp_FetchVacancyQuotaType", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }
        public async Task<List<SelectListModel>?> FetchAllQuotaDropDownAsync()
        {
            try
            {
                var param = new DynamicParameters();
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_FetchAllQuotaDropDown", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }
        public async Task<FetchVacancyCourseTypeResponse?> FetchCourseTypeAsync()
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", currentUserService.HostelId, DbType.Int64);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var innerResult = await Task.FromResult(dapper.GetAll<FetchVacancyCourseTypeList>("usp_FetchVacancyCourseType", param, commandType: CommandType.StoredProcedure));

                var result = new FetchVacancyCourseTypeResponse();
                result.CourseTypeVacancyList = innerResult;
                result.Total_Vacancy = innerResult[0].Total_Vacancy;

                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }
        public async Task<string> InsertQuotaTypeVacancyAsync(QuotaTypeObjectClass model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", currentUserService.HostelId, DbType.Int64);
                param.Add("p_SpecialVacancy", model.SpecialQuota_Vacancy_setby_warden, DbType.Int32);
                param.Add("p_CourseQuotaVacancy", model.CourseQuota_Vacancy_setby_Warden, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Get<string>("usp_InsertQuotaTypeVacancy", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }
       

        public async Task<List<FetchVacancyResponse>?> FetchVacancyListAsync(int TypeOfCourseId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", currentUserService.HostelId, DbType.Int64);
                param.Add("p_TypeOfCourseId", TypeOfCourseId, DbType.Int64);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<FetchVacancyModel>("usp_FetchRecordsForSetVacancy", param, commandType: CommandType.StoredProcedure));

                var subresult = result.DistinctBy(x => x.CasteCategoryVacancyId)
                    .Select(x => new FetchVacancyResponse
                    {
                        Actual_Vacancy = x.Actual_Vacancy,
                        Actual_Vacancy_SetBy_Warden = x.Actual_Vacancy_SetBy_Warden,
                        Calculated_Vacancy = x.Calculated_Vacancy,
                        CasteCategoryVacancyId = x.CasteCategoryVacancyId,
                        CasteCategoryVacancy_setBy_Warden = x.CasteCategoryVacancy_setBy_Warden,
                        CategoryCappingPercentage = x.CategoryCappingPercentage,
                        CourseTypeQuotaType_Vacancy = x.CourseTypeQuotaType_Vacancy,
                        Percentage = x.Percentage,
                        VacancyCategoryInPercentage = x.VacancyCategoryInPercentage,
                        VacancyCategoryName  = x.VacancyCategoryName,
                        VacancyListCasteWise = result.Where(y => y.CasteCategoryVacancyId == x.CasteCategoryVacancyId && y.CasteVacancyId.HasValue)
                                                      .Select(y => new FetchVacancyCasteWise
                                                      {
                                                          CasteVacancyId =y.CasteVacancyId,
                                                          CasteVacancyPercentage = y.CasteVacancyPercentage,
                                                          VacancyCasteName = y.VacancyCasteName,
                                                          Caste_Calculated_Vacancy = y.Caste_Calculated_Vacancy,
                                                          CasteVacancy_setBy_Warden = y.CasteVacancy_setBy_Warden
                                                      }).ToList(),
                    }).ToList();



                return subresult;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }
        public async Task UpsertVacancyAsync(VacencySetModel model)
        {
            try
            {
                foreach (var modelItem in model.Vacancies)
                {
                    if(modelItem.VacancyListCasteWise.Count > 0)
                    {
                        foreach (var item in modelItem.VacancyListCasteWise)
                        {
                            var param = new DynamicParameters();
                            param.Add("p_Id", modelItem.Id, DbType.Int32);
                            param.Add("p_Vacancy", modelItem.Vacancy, DbType.Int32);
                            param.Add("p_CasteId", item.CasteVacancyId, DbType.Int32);
                            param.Add("p_CasteVacancy", item.CasteVacancy, DbType.Int32);
                            param.Add("p_HostelId", currentUserService.HostelId, DbType.Int32);
                            param.Add("p_CourseTypeId", model.CourseTypeId, DbType.Int32);
                            param.Add("p_ActualVacancy", model.ActualVacancy, DbType.Int32);
                            param.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
                            var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                            await Task.FromResult(dapper.Execute("usp_Vacancy_Set", param, commandType: CommandType.StoredProcedure));
                        }
                    }
                    else
                    {
                        var param = new DynamicParameters();
                        param.Add("p_Id", modelItem.Id, DbType.Int32);
                        param.Add("p_Vacancy", modelItem.Vacancy, DbType.Int32);
                        param.Add("p_CasteId", null, DbType.Int32);
                        param.Add("p_CasteVacancy", null, DbType.Int32);
                        param.Add("p_HostelId", currentUserService.HostelId, DbType.Int32);
                        param.Add("p_CourseTypeId", model.CourseTypeId, DbType.Int32);
                        param.Add("p_ActualVacancy", model.ActualVacancy, DbType.Int32);
                        param.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
                        var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                        await Task.FromResult(dapper.Execute("usp_Vacancy_Set", param, commandType: CommandType.StoredProcedure));
                    } 
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
            }
        }

        public async Task UpsertCourseTypeVacancyAsync(CourseTypeSetVacancyModel model)
        {
            try
            {
                foreach (var modelItem in model.CourseTypeVacancies)
                {
                        var param = new DynamicParameters();
                        param.Add("p_Id", modelItem.Id, DbType.Int64);
                        param.Add("p_Vacancy", modelItem.Vacancy, DbType.Int32);
                        param.Add("p_HostelId", model.HostelId, DbType.Int32);
                    var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                    await Task.FromResult(dapper.Execute("usp_Set_CourseType_Vacancy", param, commandType: CommandType.StoredProcedure));
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
            }
        }
        public async Task<CapapcitySetModel?> CheckIfCapacitiesSetAsync()
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", currentUserService.HostelId, DbType.Int64);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Get<CapapcitySetModel>("usp_CheckIf_Capacities_Set", param, commandType: CommandType.StoredProcedure));

                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }
        public async Task<CapapcitySetModel?> CheckIfCapacitiesSetAsyncVjnt(int hostelId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", hostelId, DbType.Int64);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Get<CapapcitySetModel>("usp_CheckIf_Capacities_Set", param, commandType: CommandType.StoredProcedure));

                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }

        public async Task<List<FetchVacancyQuotaTypeResponse>?> FetchQuotaTypeAsyncVjnt(int hostelId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId",hostelId, DbType.Int64);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<FetchVacancyQuotaTypeResponse>("usp_FetchVacancyQuotaType", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }

        public async Task<FetchVacancyCourseTypeResponse?> FetchCourseTypeAsyncVjnt(int hostelId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", hostelId, DbType.Int64);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var innerResult = await Task.FromResult(dapper.GetAll<FetchVacancyCourseTypeList>("usp_FetchVacancyCourseType", param, commandType: CommandType.StoredProcedure));

                var result = new FetchVacancyCourseTypeResponse();
                result.CourseTypeVacancyList = innerResult;
                result.Total_Vacancy = innerResult[0].Total_Vacancy;

                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }

        public async Task<List<FetchVacancyResponse>?> FetchVacancyListAsyncVjnt(int hostelId, int TypeOfCourseId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", hostelId, DbType.Int64);
                param.Add("p_TypeOfCourseId", TypeOfCourseId, DbType.Int64);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<FetchVacancyModel>("usp_FetchRecordsForSetVacancy", param, commandType: CommandType.StoredProcedure));

                var subresult = result.DistinctBy(x => x.CasteCategoryVacancyId)
                    .Select(x => new FetchVacancyResponse
                    {
                        Actual_Vacancy = x.Actual_Vacancy,
                        Actual_Vacancy_SetBy_Warden = x.Actual_Vacancy_SetBy_Warden,
                        Calculated_Vacancy = x.Calculated_Vacancy,
                        CasteCategoryVacancyId = x.CasteCategoryVacancyId,
                        CasteCategoryVacancy_setBy_Warden = x.CasteCategoryVacancy_setBy_Warden,
                        CategoryCappingPercentage = x.CategoryCappingPercentage,
                        CourseTypeQuotaType_Vacancy = x.CourseTypeQuotaType_Vacancy,
                        Percentage = x.Percentage,
                        VacancyCategoryInPercentage = x.VacancyCategoryInPercentage,
                        VacancyCategoryName = x.VacancyCategoryName,
                        VacancyListCasteWise = result.Where(y => y.CasteCategoryVacancyId == x.CasteCategoryVacancyId && y.CasteVacancyId.HasValue)
                                                      .Select(y => new FetchVacancyCasteWise
                                                      {
                                                          CasteVacancyId = y.CasteVacancyId,
                                                          CasteVacancyPercentage = y.CasteVacancyPercentage,
                                                          VacancyCasteName = y.VacancyCasteName,
                                                          Caste_Calculated_Vacancy = y.Caste_Calculated_Vacancy,
                                                          CasteVacancy_setBy_Warden = y.CasteVacancy_setBy_Warden
                                                      }).ToList(),
                    }).ToList();



                return subresult;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }

        public async Task<string> InsertQuotaTypeVacancyAsyncVjnt(int hostelId, QuotaTypeObjectClass model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_HostelId", hostelId, DbType.Int64);
                param.Add("p_SpecialVacancy", model.SpecialQuota_Vacancy_setby_warden, DbType.Int32);
                param.Add("p_CourseQuotaVacancy", model.CourseQuota_Vacancy_setby_Warden, DbType.Int32);
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.Get<string>("usp_InsertQuotaTypeVacancy", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return null;
            }
        }
        public async Task UpsertVacancyAsyncVjnt(VacencySetModel model)
        {
            try
            {
                foreach (var modelItem in model.Vacancies)
                {
                    if (modelItem.VacancyListCasteWise.Count > 0)
                    {
                        foreach (var item in modelItem.VacancyListCasteWise)
                        {
                            var param = new DynamicParameters();
                            param.Add("p_Id", modelItem.Id, DbType.Int32);
                            param.Add("p_Vacancy", modelItem.Vacancy, DbType.Int32);
                            param.Add("p_CasteId", item.CasteVacancyId, DbType.Int32);
                            param.Add("p_CasteVacancy", item.CasteVacancy, DbType.Int32);
                            param.Add("p_HostelId", model.HostelId, DbType.Int32);
                            param.Add("p_CourseTypeId", model.CourseTypeId, DbType.Int32);
                            param.Add("p_ActualVacancy", model.ActualVacancy, DbType.Int32);
                            param.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
                            var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                            await Task.FromResult(dapper.Execute("usp_Vacancy_Set", param, commandType: CommandType.StoredProcedure));
                        }
                    }
                    else
                    {
                        var param = new DynamicParameters();
                        param.Add("p_Id", modelItem.Id, DbType.Int32);
                        param.Add("p_Vacancy", modelItem.Vacancy, DbType.Int32);
                        param.Add("p_CasteId", null, DbType.Int32);
                        param.Add("p_CasteVacancy", null, DbType.Int32);
                        param.Add("p_HostelId", model.HostelId, DbType.Int32);
                        param.Add("p_CourseTypeId", model.CourseTypeId, DbType.Int32);
                        param.Add("p_ActualVacancy", model.ActualVacancy, DbType.Int32);
                        param.Add("p_CreatedBy", currentUserService.UserId, DbType.String);
                        var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                        await Task.FromResult(dapper.Execute("usp_Vacancy_Set", param, commandType: CommandType.StoredProcedure));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
            }
        }

    }
}
