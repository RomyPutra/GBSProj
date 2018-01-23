using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Plexform.Configuration;
using Plexform.Web;

namespace Plexform.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class PlexformDbContextFactory : IDesignTimeDbContextFactory<PlexformDbContext>
    {
        public PlexformDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PlexformDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            PlexformDbContextConfigurer.Configure(builder, configuration.GetConnectionString(PlexformConsts.ConnectionStringName));

            return new PlexformDbContext(builder.Options);
        }
    }
}
