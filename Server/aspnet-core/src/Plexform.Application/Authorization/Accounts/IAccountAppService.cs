using System.Threading.Tasks;
using Abp.Application.Services;
using Plexform.Authorization.Accounts.Dto;

namespace Plexform.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
