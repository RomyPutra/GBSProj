using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Plexform.Authorization;

namespace Plexform
{
    [DependsOn(
        typeof(PlexformCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class PlexformApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<PlexformAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(PlexformApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
