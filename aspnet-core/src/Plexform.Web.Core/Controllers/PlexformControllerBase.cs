using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Plexform.Controllers
{
    public abstract class PlexformControllerBase: AbpController
    {
        protected PlexformControllerBase()
        {
            LocalizationSourceName = PlexformConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
