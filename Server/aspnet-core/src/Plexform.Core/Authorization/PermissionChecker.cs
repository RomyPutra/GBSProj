using Abp.Authorization;
using Plexform.Authorization.Roles;
using Plexform.Authorization.Users;

namespace Plexform.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
