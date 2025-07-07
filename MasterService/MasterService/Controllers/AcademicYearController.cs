using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Dynamic;

namespace MasterService.Controllers
{
    public class AcademicYearController : APIBaseController
    {
        private readonly IDapper dapper;
        private readonly ICacheService cacheService;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;


        public AcademicYearController(IDapper dapper, ICacheService cacheService, ICurrentUserService service, IErrorLogger errorLogger)
        {
            this.dapper = dapper;
            this.cacheService = cacheService;
            this.currentUserService = service;
            this.errorLogger = errorLogger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
              
                // var myparam = new DynamicParameters();
                // myparam.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
               
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetAdmissionYear", null, commandType: System.Data.CommandType.StoredProcedure));
               
                return Ok(result);
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetAdmissionYear", ex);
                return Ok(new List<string>());
            }
        }

        //private async Task<List<SelectListModel>> AdmissionYear()
        //{
        //    try
        //    {
        //        var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetAdmissionYear", null, commandType:CommandType.StoredProcedure));
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        // ExceptionLogging.LogException(Convert.ToString(ex));
        //        return new List<SelectListModel>();
        //    }
        //}

        [HttpGet("Dashboard-Manual-Depart-Scheme")]
        public async Task<IActionResult> Get(int departmentId, int schemeId)
        {
            try   
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_departmentId", departmentId, DbType.String);
                myparam.Add("p_schemeId", schemeId, DbType.String);

                var result = await Task.FromResult(dapper.GetAll<SelectDashboardManualModel>("usp_DashboardManualDeprtScheme", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return Ok(new { Status = true, Data = result });
               // return Ok(result);
            }
            catch (Exception ex)

            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_DashboardManualDeprtScheme", ex);
                return Ok(new List<string>());
            }
        }
        [HttpGet("available-academic-year")]
        public async Task<IActionResult> AvailableAcademicYear()
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_deptId", Convert.ToInt32(currentUserService.DeptId));


                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetAllAvailableAcademicyear", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return Ok(result);
                // return Ok(result);
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetAllAvailableAcademicyear", ex);
                return Ok(new List<string>());
            }
        }



    }

}
