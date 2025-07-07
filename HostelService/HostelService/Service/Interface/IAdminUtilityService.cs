using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IAdminUtilityService
    {
        Task<string?> GetPaymentStatusAsync(string applicationNo);
        Task<bool> VerifySuperAdminMobile(string MobileNo);
        Task<string?> GetApplicationIdAsync(string applicationNo);
        Task<UserAadhaarModel?> GetApplicationByAadharRef(string aadharRefNo);
        Task<UserAadhaarModel?> GetApplicationByEmail(string email);
        Task<UserAadhaarModel?> GetApplicationByMobile(string mobile);

        Task<string?> GetAyApplicationDetails();

        Task<int> updateClosingDate(ClosingDateModel model);

        Task<List<ServiceTypeClosingDateModel>> GetServiceTypeClosingDate();

        Task<List<ClosingDateModel1>> GetUpdateClosingDate(string DeptId);

        Task<List<ParentIdMenuMapping>> GetparentIdAddMenuMapping();

        Task<string> SaveMenubtnMapping(MenuInsertModel model);

        Task<List<RoleAccordDept?>> GetRoleListDeptid(string departmentid);

        Task<List<MenuMapping>> GetMenuListAll();

        Task<string> SaveMenuMapping(MenuMappingInsert model);


    }
}
