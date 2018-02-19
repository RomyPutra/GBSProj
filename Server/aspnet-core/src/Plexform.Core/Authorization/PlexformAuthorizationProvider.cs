using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Plexform.Authorization
{
    public class PlexformAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            foreach(var info in typeof(PermissionNames).GetFields())
            {
                context.CreatePermission(info.GetValue(info.Name).ToString());
            }
            //context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            //context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            //context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, PlexformConsts.LocalizationSourceName);
        }
    }
}
