using Dapper;
using Helper;
using HostelService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.LotteryModel;
using Repository.Implementation;
using Repository.Interface;
using Service.Implementation;
using Service.Interface;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HostelService.Controllers
{
    public class LotteryController : APIBaseController
    {
        private readonly ILotteryService lotteryService;
        private readonly ICurrentUserService currentUserService;

        public LotteryController(ILotteryService lotteryService, ICurrentUserService currentUserService)
        {
            this.lotteryService = lotteryService;
            this.currentUserService = currentUserService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int? DistrictId, int? CourseId,int? HostelId)
        {
            var data = await lotteryService.GetData(DistrictId, CourseId, HostelId);

            if (data != null)
            {
                if (data.CourseList != null && data.CourseList.Count > 0)

                    foreach (var course in data.CourseList)
                    {
                        if (data.PriorityList != null && data.PriorityList.Count > 0)
                        {
                            foreach (var priority in data.PriorityList)
                            {
                                if (data.ApplicantList != null && data.ApplicantList.Count > 0)
                                {
                                    switch (priority.Priority)
                                    {
                                        case 1:
                                            var DisabilityApplicants = data.ApplicantList.Where(x => x.CourseType == course.CourseTypeId && x.IsDisable == true).ToList();

                                            if (DisabilityApplicants != null && DisabilityApplicants.Count > 0)
                                            {
                                                foreach (var DisabilityApplicant in DisabilityApplicants)
                                                {
                                                    var prefrences = await lotteryService.GetApplicantPreference((long)DisabilityApplicant.UserId);

                                                    if (prefrences != null && prefrences.Count > 0)
                                                    {
                                                        foreach (var prefrence in prefrences)
                                                        {
                                                            var vacancyData = await lotteryService.GetHostelVacancy(course.CourseTypeId, prefrence.HostelId, priority.Priority);

                                                            if (vacancyData != null && vacancyData.TotalVacancy != vacancyData.TotalOccupiedVacancy)
                                                            {
                                                                await lotteryService.UpdateAllotment((long)DisabilityApplicant.UserId, prefrence.HostelId, DisabilityApplicant.ApplicationNo, course.CourseTypeId, DisabilityApplicant.CasteCategory, DisabilityApplicant.Caste, priority.Priority);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case 2:
                                            var orphanData = data.ApplicantList.Where(x => x.CourseType == course.CourseTypeId && x.IsOrphan == true).ToList();

                                            if (orphanData != null && orphanData.Count > 0)
                                            {
                                                foreach (var applicant in orphanData)
                                                {
                                                    var prefrences = await lotteryService.GetApplicantPreference((long)applicant.UserId);

                                                    if (prefrences != null && prefrences.Count > 0)
                                                    {
                                                        foreach (var prefrence in prefrences)
                                                        {
                                                            var vacancyData = await lotteryService.GetHostelVacancy(course.CourseTypeId, prefrence.HostelId, priority.Priority);

                                                            if (vacancyData != null && vacancyData.TotalVacancy != vacancyData.TotalOccupiedVacancy)
                                                            {
                                                                await lotteryService.UpdateAllotment((long)applicant.UserId, prefrence.HostelId, applicant.ApplicationNo, course.CourseTypeId, applicant.CasteCategory, applicant.Caste, priority.Priority);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case 3:
                                            List<int> castIds = new List<int>();
                                            castIds.Add(1115);
                                            castIds.Add(1116);
                                            castIds.Add(1118);
                                            castIds.Add(1119);
                                            castIds.Add(1122);
                                            castIds.Add(1123);
                                            castIds.Add(1124);

                                            var MangMatangCasteData = data.ApplicantList.Where(x => x.CourseType == course.CourseTypeId && x.CasteCategory == 2 && castIds.Contains(x.Caste)).ToList();

                                            if (MangMatangCasteData != null && MangMatangCasteData.Count > 0)
                                            {
                                                foreach (var applicant in MangMatangCasteData)
                                                {
                                                    var prefrences = await lotteryService.GetApplicantPreference((long)applicant.UserId);

                                                    if (prefrences != null && prefrences.Count > 0)
                                                    {
                                                        foreach (var prefrence in prefrences)
                                                        {
                                                            var vacancyData = await lotteryService.GetHostelVacancy(course.CourseTypeId, prefrence.HostelId, priority.Priority);

                                                            if (vacancyData != null && vacancyData.TotalVacancy != vacancyData.TotalOccupiedVacancy)
                                                            {
                                                                await lotteryService.UpdateAllotment((long)applicant.UserId, prefrence.HostelId, applicant.ApplicationNo, course.CourseTypeId, applicant.CasteCategory, applicant.Caste, priority.Priority);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case 4:


                                            var MehtarCasteData = data.ApplicantList.Where(x => x.CourseType == course.CourseTypeId && x.Caste == 1040).ToList();

                                            if (MehtarCasteData != null && MehtarCasteData.Count > 0)
                                            {
                                                foreach (var applicant in MehtarCasteData)
                                                {
                                                    var prefrences = await lotteryService.GetApplicantPreference((long)applicant.UserId);

                                                    if (prefrences != null && prefrences.Count > 0)
                                                    {
                                                        foreach (var prefrence in prefrences)
                                                        {
                                                            var vacancyData = await lotteryService.GetHostelVacancy(course.CourseTypeId, prefrence.HostelId, priority.Priority);

                                                            if (vacancyData != null && vacancyData.TotalVacancy != vacancyData.TotalOccupiedVacancy)
                                                            {
                                                                await lotteryService.UpdateAllotment((long)applicant.UserId, prefrence.HostelId, applicant.ApplicationNo, course.CourseTypeId, applicant.CasteCategory, applicant.Caste, priority.Priority);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;

                                        case 5:
                                            List<int> discludeCastIds = new List<int>();
                                            discludeCastIds.Add(1115);
                                            discludeCastIds.Add(1116);
                                            discludeCastIds.Add(1118);
                                            discludeCastIds.Add(1119);
                                            discludeCastIds.Add(1122);
                                            discludeCastIds.Add(1123);
                                            discludeCastIds.Add(1124);
                                            discludeCastIds.Add(1040);

                                            var OtherCasteData = data.ApplicantList.Where(x => x.CourseType == course.CourseTypeId && x.CasteCategory == 2 && !discludeCastIds.Contains(x.Caste)).ToList();

                                            if (OtherCasteData != null && OtherCasteData.Count > 0)
                                            {
                                                foreach (var applicant in OtherCasteData)
                                                {
                                                    var prefrences = await lotteryService.GetApplicantPreference((long)applicant.UserId);

                                                    if (prefrences != null && prefrences.Count > 0)
                                                    {
                                                        foreach (var prefrence in prefrences)
                                                        {
                                                            var vacancyData = await lotteryService.GetHostelVacancy(course.CourseTypeId, prefrence.HostelId, priority.Priority);

                                                            if (vacancyData != null && vacancyData.TotalVacancy != vacancyData.TotalOccupiedVacancy)
                                                            {
                                                                await lotteryService.UpdateAllotment((long)applicant.UserId, prefrence.HostelId, applicant.ApplicationNo, course.CourseTypeId, applicant.CasteCategory, applicant.Caste, priority.Priority);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case 6:
                                            var SBCCasteData = data.ApplicantList.Where(x => x.CourseType == course.CourseTypeId && x.CasteCategory == 9).ToList();
                                            if (SBCCasteData != null && SBCCasteData.Count > 0)
                                            {
                                                foreach (var applicant in SBCCasteData)
                                                {
                                                    var prefrences = await lotteryService.GetApplicantPreference((long)applicant.UserId);

                                                    if (prefrences != null && prefrences.Count > 0)
                                                    {
                                                        foreach (var prefrence in prefrences)
                                                        {
                                                            var vacancyData = await lotteryService.GetHostelVacancy(course.CourseTypeId, prefrence.HostelId, priority.Priority);

                                                            if (vacancyData != null && vacancyData.TotalVacancy != vacancyData.TotalOccupiedVacancy)
                                                            {
                                                                await lotteryService.UpdateAllotment((long)applicant.UserId, prefrence.HostelId, applicant.ApplicationNo, course.CourseTypeId, applicant.CasteCategory, applicant.Caste, priority.Priority);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case 7:
                                            var EBCOBCCasteData = data.ApplicantList.Where(x => x.CourseType == course.CourseTypeId && (x.CasteCategory == 3 || x.CasteCategory == 10)).ToList();
                                            if (EBCOBCCasteData != null && EBCOBCCasteData.Count > 0)
                                            {
                                                foreach (var applicant in EBCOBCCasteData)
                                                {
                                                    var prefrences = await lotteryService.GetApplicantPreference((long)applicant.UserId);

                                                    if (prefrences != null && prefrences.Count > 0)
                                                    {
                                                        foreach (var prefrence in prefrences)
                                                        {
                                                            var vacancyData = await lotteryService.GetHostelVacancy(course.CourseTypeId, prefrence.HostelId, priority.Priority);

                                                            if (vacancyData != null && vacancyData.TotalVacancy != vacancyData.TotalOccupiedVacancy)
                                                            {
                                                                await lotteryService.UpdateAllotment((long)applicant.UserId, prefrence.HostelId, applicant.ApplicationNo, course.CourseTypeId, applicant.CasteCategory, applicant.Caste, priority.Priority);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;

                                        case 8:
                                            var VJNTCasteData = data.ApplicantList.Where(x => x.CourseType == course.CourseTypeId && x.CasteCategory == 4).ToList();
                                            if (VJNTCasteData != null && VJNTCasteData.Count > 0)
                                            {
                                                foreach (var applicant in VJNTCasteData)
                                                {
                                                    var prefrences = await lotteryService.GetApplicantPreference((long)applicant.UserId);

                                                    if (prefrences != null && prefrences.Count > 0)
                                                    {
                                                        foreach (var prefrence in prefrences)
                                                        {
                                                            var vacancyData = await lotteryService.GetHostelVacancy(course.CourseTypeId, prefrence.HostelId, priority.Priority);

                                                            if (vacancyData != null && vacancyData.TotalVacancy != vacancyData.TotalOccupiedVacancy)
                                                            {
                                                                await lotteryService.UpdateAllotment((long)applicant.UserId, prefrence.HostelId, applicant.ApplicationNo, course.CourseTypeId, applicant.CasteCategory, applicant.Caste, priority.Priority);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case 9:
                                            var STCasteData = data.ApplicantList.Where(x => x.CourseType == course.CourseTypeId && x.CasteCategory == 5).ToList();
                                            if (STCasteData != null && STCasteData.Count > 0)
                                            {
                                                foreach (var applicant in STCasteData)
                                                {
                                                    var prefrences = await lotteryService.GetApplicantPreference((long)applicant.UserId);

                                                    if (prefrences != null && prefrences.Count > 0)
                                                    {
                                                        foreach (var prefrence in prefrences)
                                                        {
                                                            var vacancyData = await lotteryService.GetHostelVacancy(course.CourseTypeId, prefrence.HostelId, priority.Priority);

                                                            if (vacancyData != null && vacancyData.TotalVacancy != vacancyData.TotalOccupiedVacancy)
                                                            {
                                                                await lotteryService.UpdateAllotment((long)applicant.UserId, prefrence.HostelId, applicant.ApplicationNo, course.CourseTypeId, applicant.CasteCategory, applicant.Caste, priority.Priority);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
            }
            return Ok(true);
        }

        [Route("department/current-district")]
        [HttpGet]       
        public async Task<IActionResult> CurrentDistrict()
        {
            return Ok(new { Status = false, Data = currentUserService.DistrictId });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GenerateLotteryModel model)
        {
            try
            {
                if (currentUserService.RoleEntityId == "3")
                {
                    await lotteryService.GenerateGeneralMeritList(model);

                    await lotteryService.AllotmentGenerate(model);

                    return Ok(new { Status = true, Message = "Hostel allotment succesfully." });
                }
                else
                {
                    return Ok(new { Status = false, Message = "Unauthorize user access." });
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new { Status = false, Message = ex.Message });
            }
        }

        [Route("department/merit-generation-status")]
        [HttpGet]
        public async Task<IActionResult> GetMeritGenerationStatus(int? CourseId)
        {
            var result = await lotteryService.GetMeritGenerationStatus( CourseId );
            return Ok(new { Status = false, Data = result });
        }


        [Route("department/merit-generation-report")]
        [HttpGet]
        public async Task<IActionResult> meritgenerationreport(int? CourseId)
        {
            var data = await lotteryService.MeritGenerationReport(CourseId);
            if (data == null )
            {
                return Ok(new { status = false, message = "No data found" });
            }
            else
            {
                return Ok(new { status = true, message = "data found", result = data });
            }
           // return Ok(new { Status = false, Data = result });
        }

        [Route("department/merit-approve-list")]
        [HttpPost]
        public async Task<IActionResult> GetMeritApproveList([FromBody] MeritApproveModel model)
        {
            var data = await lotteryService.GetMeritApproveList(model);
            if (data != null && data?.List?.Count > 0)
            {
                return Ok(new { status = true, message = "data found", result = data });
            }
            else
            {
                return Ok(new { status = false, message = "No data found", result = data });
            }
        }

        [Route("department/submit-merit-approve-list")]
        [HttpPost]
        public async Task<IActionResult> SubmitMeritApproveList(List<MeritResponseModel> model)
        {
            var saveResult = await lotteryService.SaveMeritApproveList(model);

            if (saveResult.Split(":")[0] == "Success")
            {
                return Ok(new { status = true, message = saveResult.Split(":")[1] });
            }
            else
                return Ok(new { status = false, message = saveResult.Split(":")[1] });
        }

        [Route("department/merit-asst-reject-list")]
        [HttpPost]
        public async Task<IActionResult> GetMeritAsstRejectList([FromBody] MeritRejectModel model)
        {
            var data = await lotteryService.GetMeritAsstRejectList(model);
            if (data != null && data?.List?.Count > 0)
            {
                return Ok(new { status = true, message = "data found", result = data });
            }
            else
            {
                return Ok(new { status = false, message = "No data found", result = data });
            }
        }

        [Route("department/submit-merit-asst-reject-list")]
        [HttpPost]
        public async Task<IActionResult> SubmitMeritAsstRejectList(List<MeritResponseModel> model)
        {
            var saveResult = await lotteryService.SaveMeritAsstRejectList(model);

            if (saveResult.Split(":")[0] == "Success")
            {
                return Ok(new { status = true, message = saveResult.Split(":")[1] });
            }
            else
                return Ok(new { status = false, message = saveResult.Split(":")[1] });
        }
    }
}
