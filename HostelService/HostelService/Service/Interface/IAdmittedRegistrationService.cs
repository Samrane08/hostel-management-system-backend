using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IAdmittedRegistrationService
    {
        bool IsRecordValidate(AdmittedAadhharList model);
        Task Registration(string JsonData, string CourseType);
       
    }
}
