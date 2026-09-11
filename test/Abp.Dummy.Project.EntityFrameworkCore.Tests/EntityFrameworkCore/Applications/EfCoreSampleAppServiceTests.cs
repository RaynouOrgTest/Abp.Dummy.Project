using Abp.Dummy.Project.Samples;
using Xunit;

namespace Abp.Dummy.Project.EntityFrameworkCore.Applications;

[Collection(ProjectTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ProjectEntityFrameworkCoreTestModule>
{

}
