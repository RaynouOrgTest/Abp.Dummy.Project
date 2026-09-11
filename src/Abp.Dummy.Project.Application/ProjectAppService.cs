using Abp.Dummy.Project.Localization;
using Volo.Abp.Application.Services;

namespace Abp.Dummy.Project;

/* Inherit your application services from this class.
 */
public abstract class ProjectAppService : ApplicationService
{
    protected ProjectAppService()
    {
        LocalizationResource = typeof(ProjectResource);
    }
}
