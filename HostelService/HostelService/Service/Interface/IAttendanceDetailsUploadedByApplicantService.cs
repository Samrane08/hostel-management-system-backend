using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Service.Interface
{
    public interface IAttendanceDetailsUploadedByApplicantService
    {
        Task<TableResponseModel<AttendanceDetailsUploadedByApplicantModel>> GetApplicantAttendaceDetails(int pageIndex, int pageSize);
    }
}
