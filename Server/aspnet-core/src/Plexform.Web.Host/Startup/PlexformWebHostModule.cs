using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Plexform.Configuration;

namespace Plexform.Web.Host.Startup
{
    [DependsOn(
       typeof(PlexformWebCoreModule))]
    public class PlexformWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public PlexformWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PlexformWebHostModule).GetAssembly());
        }
    }
}
