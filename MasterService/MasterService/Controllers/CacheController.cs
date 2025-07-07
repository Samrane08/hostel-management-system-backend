using MasterService.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MasterService.Controllers
{
    public class CacheController : APIBaseController
    {
        private readonly ICacheService cacheService;
        public CacheController(IDapper dapper, ICacheService cacheService)
        {           
            this.cacheService = cacheService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await cacheService.GetAllKeys());
        }

        [HttpPost("Clear")]
        public async Task<IActionResult> Get([FromBody] string? Key)
        {
            await cacheService.Clear(Key);
            return Ok(new {Status = true,Message = "Cache Clear Successfully." });
        }
    }
}
