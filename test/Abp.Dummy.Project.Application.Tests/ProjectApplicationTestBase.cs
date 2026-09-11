using Volo.Abp.Modularity;

namespace Abp.Dummy.Project;

public abstract class ProjectApplicationTestBase<TStartupModule> : ProjectTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
