namespace Microsoft.Extensions.DependencyInjection
{
    using Microsoft.AspNetCore.Identity;

    public static class ServiceExtension
    {
        public static IdentityBuilder AddLinuxIdentity(this IServiceCollection services)
        {
            return services
                .AddIdentityCore<FuryTechs.LinuxAdmin.Identity.User>()
                .AddSignInManager<FuryTechs.LinuxAdmin.Identity.SignInManager>()
                .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<FuryTechs.LinuxAdmin.Identity.User>>()
                .AddUserStore<FuryTechs.LinuxAdmin.Identity.UserStore>()
                .AddUserManager<FuryTechs.LinuxAdmin.Identity.UserManager>()
                .AddRoles<FuryTechs.LinuxAdmin.Identity.Role>()
                .AddRoleStore<FuryTechs.LinuxAdmin.Identity.RoleStore>()
                .AddRoleManager<FuryTechs.LinuxAdmin.Identity.RoleManager>()
                ;
        }
    }
}
