using System.Reflection;
using System.Threading.Tasks;

namespace Abp.Dummy.Project.About;

public class AboutAppService : ProjectAppService, IAboutAppService
{
    public Task<ApplicationVersionDto> GetVersionAsync()
    {
        var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
            ?? assembly.GetName().Version?.ToString()
            ?? "unknown";

        return Task.FromResult(new ApplicationVersionDto
        {
            Version = version,
        });
    }
}
