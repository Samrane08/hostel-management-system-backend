using Dapper;
using MasterService.Models;
using MasterService.Service.Implemetation;
using MasterService.Service.Interface;
using MasterService.Service.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Linq;

namespace MasterService.Controllers
{
    public class PastCourseInitialDataController : APIBaseController
    {
        private readonly IDapper dapper;
        private readonly ICurrentUserService currentUserService;
        public PastCourseInitialDataController(ICurrentUserService service,IDapper dapper) 
        { 
            this.dapper = dapper;
            currentUserService = service;

        }
        [HttpGet("GetIntialData")]
        public async Task<IActionResult> GetIntialData(int langId)
        {
            try
            {
                PastCourseInitialDataModel currentCourseInitialDataModel = new PastCourseInitialDataModel();  
                var myparam = new DynamicParameters();
                myparam.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
                myparam.Add("p_langId", langId, DbType.Int32);
                var result = await dapper.MultiResult("usp_PastCourseInitialData", myparam, commandType: CommandType.StoredProcedure);
               
                if (result[0] != null)
                {
                    var data = JsonConvert.SerializeObject(result[0].FirstOrDefault());
                    currentCourseInitialDataModel.CurrentFields = JsonConvert.DeserializeObject<GetCurrentFields>(data);
                }
                if (result[1] != null)
                {
                    var data = JsonConvert.SerializeObject(result[1]);
                    currentCourseInitialDataModel.Statelist= JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                }
                if (result[2] != null)
                {
                    var data = JsonConvert.SerializeObject(result[2]);
                    currentCourseInitialDataModel.Educationmode = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                }
                
                if (result[3] != null)
                {
                    var data = JsonConvert.SerializeObject(result[3]);
                    currentCourseInitialDataModel.Admissionyear = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                }
              
                if (result[4] != null)
                {
                    var data = JsonConvert.SerializeObject(result[4]);
                    currentCourseInitialDataModel.Result = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                }
                if (result[5] != null)
                {
                    var data = JsonConvert.SerializeObject(result[5]);
                    currentCourseInitialDataModel.coursestatus = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                }
                if (result[6] != null)
                {
                    var data = JsonConvert.SerializeObject(result[6]);
                    currentCourseInitialDataModel.QualificationType = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                }

                return Ok(currentCourseInitialDataModel);
            }
            catch (Exception ex)
            {
                // ExceptionLogging.LogException(Convert.ToString(ex));
                return Ok(new List<string>());
            }
        }
    }
}
