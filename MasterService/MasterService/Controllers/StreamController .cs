using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class StreamController : APIBaseController
{
    private readonly IDapper dapper;

    public StreamController(IDapper dapper)
    {
        this.dapper = dapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int? intID,int? _LangId)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("_intID", intID, DbType.Int32); 
         
            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("USP_GetStreamList", myparam, commandType:System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
}
