using Volo.Abp.Modularity;

namespace Abp.Dummy.Project;

[DependsOn(
    typeof(ProjectDomainModule),
    typeof(ProjectTestBaseModule)
)]
public class ProjectDomainTestModule : AbpModule
{

}
