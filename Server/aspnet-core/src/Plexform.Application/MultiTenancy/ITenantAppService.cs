using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Plexform.MultiTenancy.Dto;

namespace Plexform.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
