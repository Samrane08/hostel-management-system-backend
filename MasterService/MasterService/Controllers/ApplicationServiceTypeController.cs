using Dapper;
using MasterService.Models;
using MasterService.Models.ReqModel;
using MasterService.Service.Implemetation;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers
{
   
    public class ApplicationServiceTypeController :  APIBaseController
    {
        private readonly IDapper dapper;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;

        public ApplicationServiceTypeController(IDapper dapper, ICurrentUserService currentUserService, IErrorLogger errorLogger)
        {
            this.dapper = dapper;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? deptId)
        {
            try
            {
                return Ok(await ApplicationServiceTypeList(deptId));
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new List<string>());
            }
        }

       
        private async Task<List<SelectListModel>> ApplicationServiceTypeList(int? deptId)
        {
            try
            {
                var myparam = new DynamicParameters();
                if (deptId ==null)
                    deptId = Convert.ToInt32(currentUserService.DeptId);

                   myparam.Add("p_deptId", deptId, DbType.Int32);
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_ApplicationServiceTypeMaster", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_ApplicationServiceTypeMaster", ex);
                return new List<SelectListModel>();
            }
        }

    }
}
