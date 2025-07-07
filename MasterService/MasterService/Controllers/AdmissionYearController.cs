using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class AdmissionYearController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly IErrorLogger errorLogger;
    private readonly ICurrentUserService currentUserService;

    public AdmissionYearController(IDapper dapper, IErrorLogger errorLogger, ICurrentUserService currentUserService)
    {
        this.dapper = dapper;
        this.errorLogger = errorLogger;
        this.currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);

            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetAdmissionyearList", myparam, commandType:System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetAdmissionyearList", ex);
            return Ok(new List<string>());
        }
    }
}
