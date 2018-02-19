using System.Threading.Tasks;
using Plexform.Configuration.Dto;

namespace Plexform.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
