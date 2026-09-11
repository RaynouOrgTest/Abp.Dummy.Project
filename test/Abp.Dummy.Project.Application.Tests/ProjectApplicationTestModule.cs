using Volo.Abp.Modularity;

namespace Abp.Dummy.Project;

[DependsOn(
    typeof(ProjectApplicationModule),
    typeof(ProjectDomainTestModule)
)]
public class ProjectApplicationTestModule : AbpModule
{

}
