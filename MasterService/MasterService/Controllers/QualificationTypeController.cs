using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class QualificationTypeController : APIBaseController
{
    private readonly IDapper dapper;

    public QualificationTypeController(IDapper dapper)
    {
        this.dapper = dapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
         
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("usp_GetQualificationType", null, commandType:System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
}
