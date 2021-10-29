using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuryTechs.LinuxAdmin.IdentityProvider.Controllers
{
  [Authorize]
  public class SecureController : ControllerBase
  {
    public IActionResult Index() {
      return Ok("Fetched successfully!");
    }
  }
}
