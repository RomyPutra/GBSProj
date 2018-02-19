using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Plexform.EntityFrameworkCore
{
    public static class PlexformDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<PlexformDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<PlexformDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
