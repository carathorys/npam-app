using System.Threading.Tasks;
using IdentityServer4.Validation;

namespace FuryTechs.LinuxAdmin.Core.Validators
{
    public class PamUserValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            return Task.CompletedTask;
        }
    }
}