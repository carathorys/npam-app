using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuryTechs.LinuxAdmin.Core.Controllers
{
    [Authorize]
    [Route("Sample")]
    public class SampleController: ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok();
        }
    }
}