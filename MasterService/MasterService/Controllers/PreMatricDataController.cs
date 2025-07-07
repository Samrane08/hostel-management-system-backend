using Dapper;
using MasterService.Models;
using MasterService.Models.ReqModel;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MasterService.Controllers;
public class PreMatricDataController : APIBaseController
{
    private readonly IPreSQLDapperr dapper;

    public PreMatricDataController(IPreSQLDapperr dapper)
    {
        this.dapper = dapper;
    }

    [HttpGet("Pre_GetHMS_ProfileData")]
    public async Task<IActionResult> Pre_GetHMS_ProfileData()
    {
        try
        {
            string dataBaseName= MasterUtility.GetDatabaseNameByAadharNo("4");
            var myparam = new DynamicParameters();
            myparam.Add("@AadhaarReferenceNo", "1150154586356785152", DbType.String);
            myparam.Add("@DatabaseName", dataBaseName, DbType.String);
            var result = await Task.FromResult(dapper.Get<PersonalDetailsModel>("usp_HMS_ProfileData", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(new { Status=true, PersonalDetails = result });
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new PersonalDetailsModel());
        }
    }


    [HttpGet("GetHMS_Address")]
    public async Task<IActionResult> GetHMS_Address()
    {
        try
        {
            string dataBaseName = MasterUtility.GetDatabaseNameByAadharNo("4");
            var myparam = new DynamicParameters();
            myparam.Add("@AadhaarReferenceNo", "1150154586356785152", DbType.String);
            myparam.Add("@DatabaseName", dataBaseName, DbType.String);

            var result = await Task.FromResult(dapper.Get<ApplicantAddress>("usp_HMS_FetchAddress", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(new { Status = true, Address = result });
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new ApplicantAddress());
        }
    }
    [HttpGet("GetHMS_ParentAddress")]
    public async Task<IActionResult> GetHMS_ParentAddress()
    {
        try
        {
            string dataBaseName = MasterUtility.GetDatabaseNameByAadharNo("4");
            var myparam = new DynamicParameters();
            myparam.Add("@AadhaarReferenceNo", "1150154586356785152", DbType.String);
            myparam.Add("@DatabaseName", dataBaseName, DbType.String);
            var result = await Task.FromResult(dapper.Get<ParentAddress>("usp_HMS_FetchparentAddress", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(new { Status = true, ApplicantPreSchoolRecord = result });
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new ParentAddress());
        }
    }
    [HttpGet("GetHMS_CurrentCourseData")]
    public async Task<IActionResult> GetHMS_CurrentCourseData()
    {
        try
        {
            string dataBaseName = MasterUtility.GetDatabaseNameByAadharNo("4");
            var myparam = new DynamicParameters();
            myparam.Add("@AadhaarReferenceNo", "1150154586356785152", DbType.String);
            myparam.Add("@DatabaseName", dataBaseName, DbType.String);

            var result = await Task.FromResult(dapper.GetAll<ApplicantPreSchoolRecord>("usp_HMS_CurrentCourseData", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new ApplicantPreSchoolRecord());
        }
    }

}
