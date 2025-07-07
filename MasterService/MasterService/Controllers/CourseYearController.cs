using Dapper;
using MasterService.Models;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Dynamic;

namespace MasterService.Controllers;

public class CourseYearController : APIBaseController
{
    private readonly IDapper dapper;
    private readonly ICurrentUserService currentUserService;
    private readonly IErrorLogger errorLogger;


    public CourseYearController(IDapper dapper, ICurrentUserService currentUserService, IErrorLogger errorLogger)
    {
        this.dapper = dapper;
        this.currentUserService = currentUserService;
        this.errorLogger = errorLogger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int courseId, int qualificationTypeId)
    {
        try
        {

            CurrentCourseCourseYearAndCourseDetails obj = new CurrentCourseCourseYearAndCourseDetails();
            var myparam = new DynamicParameters();
            myparam.Add("_intID", courseId, DbType.Int32);
            myparam.Add("_UserID", currentUserService.UserNumericId, DbType.Int32);
            var result = await dapper.MultiResult("usp_GetCurrentQualificationYearByUserIdAndCapdetails", myparam, commandType: CommandType.StoredProcedure);
            if (result[0] != null)
            {
                var data = JsonConvert.SerializeObject(result[0]);
                if (qualificationTypeId == 3)
                    obj.courseyears = JsonConvert.DeserializeObject<List<CommonDrp>>(data);
                else
                {
                    var courseYear = JsonConvert.DeserializeObject<List<CommonDrp>>(data);
                    obj.courseyears = courseYear.Where(x => !new[] { 7, 8 }.Contains(x.Value)).ToList();

                }

            }
            if (result[1] != null)
            {
                var data = JsonConvert.SerializeObject(result[1].FirstOrDefault());
                obj.coursedetails = JsonConvert.DeserializeObject<coursedetails>(data);

            }
            
            switch (obj.coursedetails.TypeOfCourse)
            {
                case 1:
                    if(obj.coursedetails.is_professional)
                    {
                        obj.wrongCourseMsg = "You have selected Non Professional course on personal info , But current course come under Prosessional course. Please Update course type on personal info";
                    }
                    break;
                case 2:
                    if (!obj.coursedetails.is_professional)
                    {
                       
                        obj.wrongCourseMsg = "You have selected  Professional course on personal info , But current course come under Non Prosessional course. Please Update course type on personal info";
                    }
                    break;
                default:
                    obj.wrongCourseMsg = "";
                    break;
            }
            
           
            return Ok(obj);
        }
        catch (Exception ex)
        {
            // ExceptionLogging.LogException(Convert.ToString(ex));
            await errorLogger.Log("usp_GetCurrentQualificationYearByUserIdAndCapdetails", ex);
            return BadRequest();
        }
    }
}
