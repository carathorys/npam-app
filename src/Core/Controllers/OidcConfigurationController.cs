using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FuryTechs.LinuxAdmin.Core.Controllers
{
    public class OidcConfigurationController : ControllerBase
    {
        private readonly ILogger<OidcConfigurationController> _logger;

        public OidcConfigurationController(ILogger<OidcConfigurationController> logger)
        {
            _logger = logger;
        }

        [HttpGet("_configuration/{clientId}")]
        public IActionResult GetClientRequestParameters([FromRoute] string clientId)
        {
            return Ok(new
            {
                authority= "https://localhost:5001/",
                client_id= "js",
                redirect_uri= "https://localhost:5001/authentication/login-callback",
                post_logout_redirect_uri= "https://localhost:5001/authentication/logout-callback",
                response_type= "password",
                scope= "openid profile api1",
                filterProtocolClaims= true,
                loadUserInfo= true
            });
        }
    }
}