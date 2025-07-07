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
               
                var saveResult = await pastcourseservice.SaveAsync(model);

                if (saveResult > 0)
                {
                    model.rowId= saveResult;
                    return Ok(new { status = true, message = "Data saved successfully", Data = model });
                }
                else
                    return Ok(new { status = false, message = "Some error occured", Data = model });

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
                List<CourseDetailsTblViewModel> result = null;
                result = await pastcourseservice.GetAsync();

                if (result == null || result.Count < 1)
                {
                    string UIDINo = ""; string FirstDigit = "";
                    var registerDetails = await  currentcourseService.GetAadharAndCheckShouldFetchRecordFromPost("past");
                    if (!string.IsNullOrWhiteSpace(registerDetails))
                    {
                        var qs = QueryStringEncryptDecrypt.DecryptQueryString(registerDetails);
                        string UID = qs["UID"];
                        UIDINo = !string.IsNullOrWhiteSpace(UID) ? UID : "";
                        FirstDigit = UIDINo.Substring(0, 1);
                        UIDINo = QueryStringEncryptDecrypt.EncryptAadhaar(UIDINo);
                        var PostReqModel = new PostUtility(UIDINo, FirstDigit);
                       
                        var postResult = await httpClientService.RequestSendWithHeader<PostApiModel>(HttpMethod.Post, $"{urloptions.PostService}/hms/GetHMS_PastCourseData", PostReqModel);

                        if (postResult != null && postResult.PastCourseDetails != null && postResult.PastCourseDetails.Count > 0)
                        {
                            foreach (var o in postResult.PastCourseDetails)
                            {
                                await pastcourseservice.SaveAsync(o);
                            }

                        }
                    }
                    result = await pastcourseservice.GetAsync();
                }
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
      
        [HttpPost("SaveApplicantPrepastSchoolRecord")]
        public async Task<IActionResult> SaveApplicantPrepastSchoolRecord(ApplicantPreschoolRecordPast model)
        {
            try
            {
                var saveResult = await pastcourseservice.SavePrePast(model);

                if (saveResult > 0)
                {
                    model.Id = saveResult;
                    return Ok(new { status = true, message = "data saved successfully", Data = model });
                }
                else
                    return Ok(new { status = false, message = "Some error occured", Data = model });

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = "some error occured", Data = "[]" });
            }
        }
        [HttpGet("GetPre_pastSchoolRecords")]
        public async Task<IActionResult> GetPre_pastSchoolRecords()
        {
            try
            {
                var result = await pastcourseservice.GetPrePast();

                if (result.Count > 0)
                {
                    return Ok(new { status = true, message = "data got successfully", Data = result });
                }
                else
                    return Ok(new { status = false, message = "No record found", Data = "[]" });

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = "some error occured", Data = "[]" });
            }
        }
        [HttpPost("delete_pastPre")]
        public async Task<IActionResult> deletePreSchoolRecord(reqModel m)
        {
            try
            {
                var result = await pastcourseservice.DeletePrePast(m.rowId);
                return Ok(new { status = true, message = "data deleted successfully", Data = result });


            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = "some error occured", Data = "[]" });
            }
        }
        [HttpGet("CheckIsPreOrPost")]
        public async Task<IActionResult> CheckIsPreOrPost()
        {
            try
            {
                var result = await pastcourseservice.CheckIsPreOrPost();
                return Ok(new { status = true, message = "data fetched", Data = result });


            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = "some error occured", Data = "[]" });
            }
        }
        [HttpGet("GetMininumEntryYear")]
        public async Task<IActionResult> GetMininumEntryYear()
        {
            try
            {
                var result = await pastcourseservice.GetMininumEntryYear();
                return Ok(new { status = true, message = "data fetched", Data = result });


            }
            catch (Exception ex)
            {
                return Ok(new { status = false, message = "some error occured", Data = "[]" });
            }
        }
    }
}
