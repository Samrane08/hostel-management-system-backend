using Dapper;
using MasterService.Models;
using MasterService.Service.Implementation;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace MasterService.Controllers;

public class MPSCController : APIBaseController
{
    private readonly IDapper dapper;

    public MPSCController(IDapper dapper)
    {
        this.dapper = dapper;
    }
   
    [HttpGet("basic-details")]
    public async Task<IActionResult> GetmpscBasicDetails()
    {
        try
        {
            var m = new MPSCBasicDetails();
            var myparam = new DynamicParameters();
            myparam.Add("p_caller", 0, DbType.Int32);
            var result = await dapper.MultiResult("usp_mpsc_Basic_Details_Data", myparam, commandType: CommandType.StoredProcedure);

            if (result[0] != null)
            {
                var data = JsonConvert.SerializeObject(result[0]);
                m.CasteCategory = JsonConvert.DeserializeObject<List<SelectList>>(data);
            }
            if (result[1] != null)
            {
                var data = JsonConvert.SerializeObject(result[1]);
                m.coaching = JsonConvert.DeserializeObject<List<SelectList>>(data);
            }
            if (result[2] != null)
            {
                var data = JsonConvert.SerializeObject(result[2]);
                m.gender = JsonConvert.DeserializeObject<List<SelectList>>(data);
            }
            if (result[3] != null)
            {
                var data = JsonConvert.SerializeObject(result[3]);
                m.interviewsponsorship = JsonConvert.DeserializeObject<List<SelectList>>(data);
            }
            if (result[4] != null)
            {
                var data = JsonConvert.SerializeObject(result[4]);
                m.mainsponsorship = JsonConvert.DeserializeObject<List<SelectList>>(data);
            }
            if (result[5] != null)
            {
                var data = JsonConvert.SerializeObject(result[5]);
                m.options = JsonConvert.DeserializeObject<List<SelectList>>(data);
            }
            if (result[6] != null)
            {
                var data = JsonConvert.SerializeObject(result[6]);
                m.sponsorship = JsonConvert.DeserializeObject<List<SelectList>>(data);
            }
            if (result[7] != null)
            {
                var data = JsonConvert.SerializeObject(result[7]);
                m.sponsorship_10000 = JsonConvert.DeserializeObject<List<SelectList>>(data);
            }
            return Ok(m);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }

    [HttpGet("personal-details")]
    public async Task<IActionResult> GetmpscPersonalDetails()
    {
        try
        {
            var m = new MPSCBasicDetails();
            var myparam = new DynamicParameters();
            myparam.Add("p_caller", 1, DbType.Int32);
            var result = await dapper.MultiResult("usp_mpsc_Basic_Details_Data", myparam, commandType: CommandType.StoredProcedure);

            if (result[0] != null)
            {
                var data = JsonConvert.SerializeObject(result[0]);
                m.CasteCategory = JsonConvert.DeserializeObject<List<SelectList>>(data);
            }
            
            if (result[1] != null)
            {
                var data = JsonConvert.SerializeObject(result[1]);
                m.gender = JsonConvert.DeserializeObject<List<SelectList>>(data);
            }
           
            if (result[2] != null)
            {
                var data = JsonConvert.SerializeObject(result[2]);
                m.options = JsonConvert.DeserializeObject<List<SelectList>>(data);
            }
           
            return Ok(m);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
    [HttpGet("other-details")]
    public async Task<IActionResult> GetmpscOtherDetails()
    {
        try
        {
            var m = new MPSCBasicDetails();
            var myparam = new DynamicParameters();
            myparam.Add("p_caller", 2, DbType.Int32);
            var result = await dapper.MultiResult("usp_mpsc_Basic_Details_Data", myparam, commandType: CommandType.StoredProcedure);

            if (result[0] != null)
            {
                var data = JsonConvert.SerializeObject(result[0]);
                m.options = JsonConvert.DeserializeObject<List<SelectList>>(data);
            }

           

            return Ok(m);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
    [HttpGet("caste-category")]
    public async Task<IActionResult> CasteCategory()
    {
        try
        {
           
            var result = await Task.FromResult(dapper.GetAll<SelectList>("usp_mpsc_CasteCategory", null,commandType:System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
    [HttpGet("coaching")]
    public async Task<IActionResult>CoachingList()
    {
        try
        {

            var result = await Task.FromResult(dapper.GetAll<SelectList>("usp_GetCoachinglist", null, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
    [HttpGet("gender")]
    public async Task<IActionResult> Gender()
    {
        try
        {

            var result = await Task.FromResult(dapper.GetAll<SelectList>("usp_GetMPSCGender", null, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
    [HttpGet("interview-sponsorship")]
    public async Task<IActionResult> InterviewSponsorship()
    {
        try
        {

            var result = await Task.FromResult(dapper.GetAll<SelectList>("usp_GetMPSCinterviewsponsorship", null, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
    [HttpGet("main-sponsorship")]
    public async Task<IActionResult> MainSponsorship()
    {
        try
        {

            var result = await Task.FromResult(dapper.GetAll<SelectList>("usp_Getmainsponsorship", null, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
    [HttpGet("options")]
    public async Task<IActionResult> Options()
    {
        try
        {

            var result = await Task.FromResult(dapper.GetAll<SelectList>("usp_GetMPSCOptions", null, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
    [HttpGet("sponsorship")]
    public async Task<IActionResult> Sponsorship()
    {
        try
        {

            var result = await Task.FromResult(dapper.GetAll<SelectList>("usp_GetMPSCSponsorshipProgram", null, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
    [HttpGet("sponsorship_10000")]
    public async Task<IActionResult> Sponsorship_10000()
    {
        try
        {

            var result = await Task.FromResult(dapper.GetAll<SelectList>("usp_Getmpscsponsorshipprogrammaster_10000", null, commandType: System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
}
