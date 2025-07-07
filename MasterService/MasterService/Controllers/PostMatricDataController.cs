using Dapper;
using MasterService.Models;
using MasterService.Models.ReqModel;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MasterService.Controllers;
public class PostMatricDataController : APIBaseController
{
    private readonly ISQLDapper dapper;

    public PostMatricDataController(ISQLDapper dapper)
    {
        this.dapper = dapper;
    }

    [HttpGet("GetHMS_ProfileData")]
    public async Task<IActionResult> GetHMS_ProfileData()
    {
        try
        {
            string dataBaseName= MasterUtility.GetDatabaseNameByAadharNo("5");
            var myparam = new DynamicParameters();
            myparam.Add("@AadhaarNo", "5070A5F4622F643BBB45E100E65990A13DA981958AEDA389", DbType.String);
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
    [HttpGet("GetHMS_PastCourseData")]
    public async Task<IActionResult> GetHMS_PastCourseData()
    {
        try
        {
            string dataBaseName = MasterUtility.GetDatabaseNameByAadharNo("5");
            var myparam = new DynamicParameters();
            myparam.Add("@AadhaarNo", "5070A5F4622F643BBB45E100E65990A13DA981958AEDA389", DbType.String);
            myparam.Add("@DatabaseName", dataBaseName, DbType.String);

            var result = await Task.FromResult(dapper.GetAll<EducationDetails>("usp_HMS_PastCourseData", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new EducationDetails());
        }
    }

    [HttpGet("GetHMS_Address")]
    public async Task<IActionResult> GetHMS_Address()
    {
        try
        {
            string dataBaseName = MasterUtility.GetDatabaseNameByAadharNo("5");
            var myparam = new DynamicParameters();
            myparam.Add("@AadhaarNo", "5070A5F4622F643BBB45E100E65990A13DA981958AEDA389", DbType.String);
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
            string dataBaseName = MasterUtility.GetDatabaseNameByAadharNo("5");
            var myparam = new DynamicParameters();
            myparam.Add("@AadhaarNo", "5070A5F4622F643BBB45E100E65990A13DA981958AEDA389", DbType.String);
            myparam.Add("@DatabaseName", dataBaseName, DbType.String);

            var result = await Task.FromResult(dapper.Get<ParentAddress>("usp_HMS_FetchParentAddress", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(new { Status = true, ParentAddress = result });
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new ApplicantAddress());
        }
    }
    [HttpGet("GetHMS_CurrentCourseData")]
    public async Task<IActionResult> GetHMS_CurrentCourseData()
    {
        try
        {
            string dataBaseName = MasterUtility.GetDatabaseNameByAadharNo("5");
            var myparam = new DynamicParameters();
            myparam.Add("@AadhaarNo", "5070A5F4622F643BBB45E100E65990A13DA981958AEDA389", DbType.String);
            myparam.Add("@DatabaseName", dataBaseName, DbType.String);

            var result = await Task.FromResult(dapper.GetAll<CurrentCourseDetails>("usp_HMS_CurrentCourseData", myparam, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new CurrentCourseDetails());
        }
    }

}
