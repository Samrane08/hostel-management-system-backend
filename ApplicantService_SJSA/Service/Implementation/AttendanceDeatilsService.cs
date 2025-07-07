using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Helper;
using Model;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation
{
    public class AttendanceDeatilsService : IAttendanceDetails
    {
        private readonly IDapper dapper;
        private readonly ICurrentUserService currentUserService;
        private readonly IErrorLogger errorLogger;

        public AttendanceDeatilsService(IDapper dapper, ICurrentUserService currentUserService, IErrorLogger errorLogger)
        {
            this.dapper = dapper;
            this.currentUserService = currentUserService;
            this.errorLogger = errorLogger;
        }
        //test comment

        public async Task<string> SaveAttendanceDetailsAsync(AttendanceDetailsModel model)
        {
            try
            {
                var myparam = new DynamicParameters();
                myparam.Add("p_UserId", currentUserService.UserNumericId, DbType.Int32);
                myparam.Add("p_AcademicYear", model.AcademicYear, DbType.String);
                myparam.Add("p_InstallmentNo", model.InstallmentNo, DbType.String);
                myparam.Add("p_FileId", model.AttendanceDoc, DbType.String);
                myparam.Add("p_AttendancePercentage", model.AttendancePercentage, DbType.String);
                myparam.Add("p_PercentageOfMarks", model.PercentageOfMarks, DbType.String);
                myparam.Add("p_isPostMatricScholarship", model.isPostMatricScholarship, DbType.String);
                myparam.Add("p_PostMatricScholarshipAmount", model.PostMatricScholarshipAmount, DbType.String);

                var result = await Task.FromResult(dapper.Insert<string>("usp_InsertApplicantAttendanceDetails", myparam, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<AttendanceDetailsModel?>> GetApplicantAttendaceDetails()
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_UserId", currentUserService.UserNumericId, DbType.Int64);
                var result = await Task.FromResult(dapper.GetAll<AttendanceDetailsModel>("usp_GetApplicantAttendaceDetails", param, commandType: CommandType.StoredProcedure));
                return result;

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(Convert.ToString(ex));
                await errorLogger.Log("usp_GetApplicantAttendaceDetails", ex);
                return null;
            }
        }

    }
}
