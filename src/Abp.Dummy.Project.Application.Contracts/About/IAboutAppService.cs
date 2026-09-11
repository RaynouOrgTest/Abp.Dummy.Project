using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Abp.Dummy.Project.About;

public interface IAboutAppService : IApplicationService
{
    Task<ApplicationVersionDto> GetVersionAsync();
}
