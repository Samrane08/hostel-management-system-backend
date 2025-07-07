using HostelService.Helper;
using HostelService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Model;
using Model.LotteryModel;
using Repository.Interface;
using Service.Implementation;
using Service.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HostelService.Controllers
{

    public class AttendanceDetailsUploadedByApplicantController : APIBaseController
    {

        private readonly IHttpClientService httpClientService;
        private readonly APIUrl urloptions;
        private readonly ICurrentUserService currentUserService;
        private readonly IAttendanceDetailsUploadedByApplicantService attendanceDetailsUploadedByApplicantService;

        public AttendanceDetailsUploadedByApplicantController(IAttendanceDetailsUploadedByApplicantService attendanceDetailsUploadedByApplicantService, IHttpClientService httpClientService, IOptions<APIUrl> urloptions, ICurrentUserService currentUserService)
        {
            this.httpClientService = httpClientService;
            this.urloptions = urloptions.Value;
            this.currentUserService = currentUserService;
            this.attendanceDetailsUploadedByApplicantService = attendanceDetailsUploadedByApplicantService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageIndex, int pageSize)
        {
            return Ok(await attendanceDetailsUploadedByApplicantService.GetApplicantAttendaceDetails(pageIndex, pageSize));
        }
    }
}
