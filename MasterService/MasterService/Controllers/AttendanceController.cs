using Dapper;
using MasterService.Models;
using MasterService.Service.Implemetation;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Dynamic;

namespace MasterService.Controllers
{
    public class AttendanceController : APIBaseController
    {
        private readonly IDapper dapper;
        private readonly IErrorLogger errorLogger;


        public AttendanceController(IDapper dapper, IErrorLogger errorLogger)
        {
            this.dapper = dapper;
            this.errorLogger = errorLogger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int departmentId, int serviceType)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_departmentId", departmentId, DbType.Int64);
                myparam.Add("p_ServiceType", serviceType, DbType.Int64);
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetInstallmentList", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return Ok(result);
            }
            catch (Exception ex)
            {
                await errorLogger.Log("usp_GetInstallmentList", ex);
                return Ok(new List<string>());
            }
        }

        [HttpGet("available-academic-year")]
        public async Task<IActionResult> AvailableAcademicYear(int departmentId)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_deptId", departmentId);
                var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetAllAvailableAcademicyear", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return Ok(result);
            }
            catch (Exception ex)
            {
                await errorLogger.Log("usp_GetAllAvailableAcademicyear", ex);
                return Ok(new List<string>());
            }
        }
    }

}
