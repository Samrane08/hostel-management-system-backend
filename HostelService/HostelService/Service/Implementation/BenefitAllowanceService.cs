using Dapper;
using Helper;
using Model;
using Repository.Interface;
using Service.Interface;
using System.Data;

namespace Service.Implementation
{
    public class BenefitAllowanceService: IBenefitAllowanceService
    {
        private readonly IDapperResolver dapperresolverInstance;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;

        public BenefitAllowanceService(IDapperResolver dapperresolverInstance, ICurrentUserService currentUserService,IErrorLogger errorLogger)
        {
            this.dapperresolverInstance = dapperresolverInstance;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }
        
        public async Task<List<BenefitAloowanceCategoryModel>> GetListAsync()
        {
            try
            {               
                var param = new DynamicParameters();
                var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                var result = await Task.FromResult(dapper.GetAll<BenefitAloowanceCategoryModel>("usp_GetBenefitAllowanceCategories", param, commandType: CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetBenefitAllowanceCategories", ex);
                return new List<BenefitAloowanceCategoryModel>();
            }
        }

        public async Task<bool> UsertAsync(BenefitAloowanceCategoryInsertModel model)
        {
            try
            {
                foreach (var item in model.VillegeIds)
                {
                    var param = new DynamicParameters();
                    param.Add("p_CategoryId", model.CategoryId, DbType.Int32);
                    param.Add("p_Village", item, DbType.Int32);
                    var dapper = dapperresolverInstance.Resolve(currentUserService.DeptId);
                    var result = await Task.FromResult(dapper.Execute("usp_UpsertBenefitAllowance", param, commandType: CommandType.StoredProcedure));                   
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_UpsertBenefitAllowance", ex);
                return false;
            }
        }
    }
}
