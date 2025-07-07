using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IWardenProfileService
    {
        Task<WardenProfileModel?> GetWardenProfile();

        Task<WardenProfileModel?> SaveWardenProfile(WardenProfileModel model);
        Task<List<ResponseNoticeModel?>> GetNotices();
        Task<ResponseNoticeModel> SaveNotices(ReqNoticeModel model);
        Task<bool> DeleteNitice(long Id);
        Task WardenMap(WardenMapModel model);       
    }
}
