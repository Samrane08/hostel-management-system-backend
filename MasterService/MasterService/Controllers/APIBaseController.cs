using Microsoft.AspNetCore.Mvc;

namespace MasterService.Controllers;

[Route("master-service/api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "MasterService")]
public class APIBaseController : ControllerBase
{
}
