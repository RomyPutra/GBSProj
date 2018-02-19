using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Plexform.Authorization.Roles;
using Plexform.Authorization.Users;
using Plexform.MultiTenancy;

namespace Plexform.EntityFrameworkCore
{
    public class PlexformDbContext : AbpZeroDbContext<Tenant, Role, User, PlexformDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public PlexformDbContext(DbContextOptions<PlexformDbContext> options)
            : base(options)
        {
        }
    }
}
