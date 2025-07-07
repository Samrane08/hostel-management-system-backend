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
    public class CurrentCourseInitialDataController : APIBaseController
    {
        private readonly IDapper dapper;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;

        public CurrentCourseInitialDataController(ICurrentUserService service,IDapper dapper, IErrorLogger errorLogger) 
        { 
            this.dapper = dapper;
            currentUserService = service;
            this.errorLogger = errorLogger;

        }
        [HttpGet("GetIntialData")]
        public async Task<IActionResult> GetIntialData(int langId,string stateCode)
        {
            CurrentCourseInitialDataModel currentCourseInitialDataModel = new CurrentCourseInitialDataModel();
            try
            {
                //CurrentCourseInitialDataModel currentCourseInitialDataModel = new CurrentCourseInitialDataModel();
                var myparam = new DynamicParameters();
                myparam.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
                myparam.Add("p_statecode", stateCode, DbType.Int32);
                myparam.Add("p_langId", langId, DbType.Int32);
                var result = await dapper.MultiResult("usp_CurrentCourseInitialData", myparam, commandType: CommandType.StoredProcedure);

                if (result[0] != null)
                {
                    var data = JsonConvert.SerializeObject(result[0].FirstOrDefault());
                    currentCourseInitialDataModel.IsNewApplicant = JsonConvert.DeserializeObject<IsNewApplicantModel>(data);
                }
                if (currentCourseInitialDataModel.IsNewApplicant.IsNewApplicant != null)
                {
                    currentCourseInitialDataModel.IsDataAvail=true;
                    if (result[1] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[1]);
                        currentCourseInitialDataModel.Statelist = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                    }
                    if (result[2] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[2]);
                        currentCourseInitialDataModel.Educationmode = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                    }
                    if (result[3] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[3]);
                        currentCourseInitialDataModel.AdmissionType = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                    }
                    if (result[4] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[4]);
                        currentCourseInitialDataModel.Admissionyear = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                    }
                    if (result[5] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[5]);
                        currentCourseInitialDataModel.Academic_year = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                    }
                    if (result[6] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[6]);
                        currentCourseInitialDataModel.Vjnt_Academic_year = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                    }
                    if (result[7] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[7]);
                        currentCourseInitialDataModel.District = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                    }
                    if (result[8] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[8]);
                        currentCourseInitialDataModel.Result = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                    }
                    if (result[9] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[9]);
                        currentCourseInitialDataModel.Coursetype = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                    }
                    if (result[10] != null)
                    {
                        var data = JsonConvert.SerializeObject(result[10]);
                        currentCourseInitialDataModel.Qualificationtype = JsonConvert.DeserializeObject<List<CommonDrp>>(data);

                    }
                }else
                {
                    currentCourseInitialDataModel.IsDataAvail = false;
                    currentCourseInitialDataModel.Message = "Please fill the personal details first";


                }
                    return Ok(currentCourseInitialDataModel);
                
            }
            catch (Exception ex)
            {
                currentCourseInitialDataModel.IsDataAvail = false;
                currentCourseInitialDataModel.Message = "Something went wrong. Please try again later";
               // ExceptionLogging.LogException(Convert.ToString(ex));
               await errorLogger.Log("usp_CurrentCourseInitialData", ex);
                return Ok(currentCourseInitialDataModel);
            }
        }
    }
}
