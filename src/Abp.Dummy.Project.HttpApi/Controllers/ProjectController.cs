using Abp.Dummy.Project.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Abp.Dummy.Project.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ProjectController : AbpControllerBase
{
    protected ProjectController()
    {
        LocalizationResource = typeof(ProjectResource);
    }
}
