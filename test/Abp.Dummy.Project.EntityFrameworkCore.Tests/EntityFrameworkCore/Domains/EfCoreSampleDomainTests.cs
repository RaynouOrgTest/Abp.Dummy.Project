using Abp.Dummy.Project.Samples;
using Xunit;

namespace Abp.Dummy.Project.EntityFrameworkCore.Domains;

[Collection(ProjectTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ProjectEntityFrameworkCoreTestModule>
{

}
