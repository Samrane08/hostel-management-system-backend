using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MasterService.Controllers;

public class UniversityController : APIBaseController
{
    private readonly IDapper dapper;

    public UniversityController(IDapper dapper)
    {
        this.dapper = dapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int ? courseId,int? colId,int?qualificationLvl,int? langId = 1)
    {
        try
        {
            var myparam = new DynamicParameters();
            myparam.Add("intID", colId, DbType.Int32);
            myparam.Add("intID_extra", courseId, DbType.Int32);
            myparam.Add("qualificationLvl", qualificationLvl, DbType.Int32);
            myparam.Add("LangId", langId, DbType.Int32);

            var result = await Task.FromResult(dapper.GetAll<SelectListModel>("GetUniversityInfo", myparam, commandType:System.Data.CommandType.StoredProcedure));
            return Ok(result);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            return Ok(new List<string>());
        }
    }
}
