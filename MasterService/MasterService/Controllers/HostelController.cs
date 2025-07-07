using Dapper;
using MasterService.Models;
using MasterService.Models.ReqModel;
using MasterService.Service.Implemetation;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class HostelController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly ICacheService cacheService;
    private readonly IErrorLogger errorLogger;
    public HostelController(IDapper dapper, ICacheService cacheService, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.cacheService = cacheService;
        this.errorLogger = errorLogger;
    }
    [HttpGet]
    public async Task<IActionResult> Get(int? dist,int? taluka)
    {
        try
        {
            var response = new List<SelectListModel>();
            var result = await cacheService.GetOrCreateAsync("HostelMaster", async () => { return await HostelList(); }, TimeSpan.FromDays(30));

            if (result != null && result.Count > 0 && dist.HasValue)
                result = result.Where(x => x.DistrictID == dist).ToList();

            if (result != null && result.Count > 0 && taluka.HasValue)
                result = result.Where(x => x.TalukaID == taluka).ToList();

            if (result != null && result.Count > 0)
                response = result.Select(x => new SelectListModel { Value = x.HostelID.ToString(), Text = x.HostelName })
                                 .ToList();

            if(response.Count == 0)
            {
                var data = await HostelList();

                if (dist.HasValue)
                    data = data.Where(x => x.DistrictID == dist).ToList();

                if (taluka.HasValue)
                    data = data.Where(x => x.TalukaID == taluka).ToList();

                
                 response = data.Select(x => new SelectListModel { Value = x.HostelID.ToString(), Text = x.HostelName })
                                     .ToList();
                return Ok(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }

    [HttpGet("List")]
    public async Task<IActionResult> List()
    {
        try
        {           
            var result = await Task.FromResult(dapper.GetAll<object>("select HostelID,DDO_code_number_in_English_10_digit_number_Sample_2001015607 as DDOCode,Name_of_Warden_in_English as Name,Mobile from mst_hostel", null, commandType: CommandType.Text));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }

    private async Task<List<HostelList>> HostelList()
    {
        try
        {   //usp_GetTalukaList
            var result = await Task.FromResult(dapper.GetAll<HostelList>("usp_HostelMaster", null, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetAllAvailableAcademicyear", ex);
            return new List<HostelList>();
        }
    }
}
