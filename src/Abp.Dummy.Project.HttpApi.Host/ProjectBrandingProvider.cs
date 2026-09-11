using Microsoft.Extensions.Localization;
using Abp.Dummy.Project.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Abp.Dummy.Project;

[Dependency(ReplaceServices = true)]
public class ProjectBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ProjectResource> _localizer;

    public ProjectBrandingProvider(IStringLocalizer<ProjectResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
