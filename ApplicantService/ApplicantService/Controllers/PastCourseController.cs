using ApplicantService.Helper;
using ApplicantService.Service;
using Dapper;
using Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Service.Implementation;
using Service.Interface;
using System.ComponentModel;
using System.Data;
using VJNTRules;

namespace ApplicantService.Controllers
{
    public class PastCourseController : APIBaseController
    {
      
        private readonly IPastCourseService pastcourseservice;
        private readonly IHttpClientService httpClientService;
        private readonly ICurrentCourseService currentcourseService;
        private readonly APIUrl urloptions;

        public PastCourseController(IPastCourseService pastcourseservice, IHttpClientService httpClientService, ICurrentCourseService currentcourseService,
        IOptions<APIUrl> urloptions)
        {
         
            this.pastcourseservice = pastcourseservice;
            this.httpClientService = httpClientService;
            this.currentcourseService = currentcourseService;
            this.urloptions = urloptions.Value;
        }
        [HttpPost("saveeducationdetails")]
        public async Task<IActionResult> saveeducationdetails(EducationDetails model)
        {
            try
            {
                var result = PastCourseRules.SaveVJNTPastCourseRules(model);
                if (result.status)
                {


                    var saveResult = await pastcourseservice.SaveAsync(model);


                    if (saveResult > 0)
                    {
                        model.rowId = saveResult;
                        return Ok(new { status = true, message = "Data saved successfully", Data = model });
                    }
                    else
                        return Ok(new { status = false, message = "Some error occured", Data = model });
                }else
                {
                    return Ok(new { status = false, message =result.message, Data = model });
                }

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message }) ;
            }
        }




        [HttpGet("get-pasteducation")]
        public async Task<object> Getpasteducation()
        {
            try
            {

                List<CourseDetailsTblViewModel> result = await pastcourseservice.GetAsync();

             
                if (result.Count > 0)
                    return Ok(new { status = true, message = "Data fetched successfully", PastEducation = result });
                else
                    return Ok(new { status = false, message = "No data found on current filters" });


            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }
        [HttpPost("delete-pastCourse")]
        public async Task<object> deletepastcurse([FromBody] reqModel m)
        {
            try
            {
                var result = await pastcourseservice.DeleteAsync(m.rowId);

                return Ok(new { status = true, message = "data deleted successfully", data = result });



            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = ex.Message });
            }
        }
      
     
      
        
        
        
    }
}
