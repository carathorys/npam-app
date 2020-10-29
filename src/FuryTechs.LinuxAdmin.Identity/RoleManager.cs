using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FuryTechs.LinuxAdmin.Identity
{
    public class RoleManager : AspNetRoleManager<Role>
    {
        public RoleManager(
            IRoleStore<Role> store,
            IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<Role>> logger,
            IHttpContextAccessor contextAccessor) : base(
            store,
            roleValidators,
            keyNormalizer,
            errors,
            logger,
            contextAccessor
        )
        {
        }
    }
}