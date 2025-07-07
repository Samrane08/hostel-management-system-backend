using Helper;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service.Interface;

namespace HostelService.Controllers
{
    public class ReportController : APIBaseController
    {
        private readonly IReportService reportService;
        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }
        [HttpPost]      
        public async Task<IActionResult> Post([FromBody] ReportFilterModel model)
        {
            return Ok(await reportService.GetListAsync(model));
        }

        [HttpGet("export-to-excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {

                var data = await reportService.GetListAsyncForWarden();
                if (data == null || data.Count == 0)
                {
                    return Ok(new { status = false, messgae = "No data found on this hostel id" });
                }
                else
                {
                    return Ok(new { status = true, messgae = "data found", result = data });
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return BadRequest(new { status = false, messgae = "Internal server error" });
            }
        }

        [HttpGet("get-daily-reports")]
        public async Task<IActionResult> GetDailyReports(int? reportId)
        {
            try
            {
                var data = await reportService.GetDailyReports(reportId);
                if (data == null || data.Count == 0)
                {
                    return Ok(new { status = false, message = "No data found" });
                }
                else
                {
                    return Ok(new { status = true, message = "data found", result = data });
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                return BadRequest(new { status = false, message = "Internal server error" });
            }
        }

    }
}
