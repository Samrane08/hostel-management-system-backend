using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Service.Interface
{
    public interface IAttendanceDetails
    {
        //test comment
        Task<string> SaveAttendanceDetailsAsync(AttendanceDetailsModel model);

        Task<List<AttendanceDetailsModel?>> GetApplicantAttendaceDetails();
    }
}
