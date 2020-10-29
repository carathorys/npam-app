using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FuryTechs.LinuxAdmin.Identity
{
    public class ClaimsPrincipalFactory: IUserClaimsPrincipalFactory<User>
    {
        public Task<ClaimsPrincipal> CreateAsync(User user)
        {
            
            return Task.FromResult(ClaimsPrincipal.Current);
        }
    }
}