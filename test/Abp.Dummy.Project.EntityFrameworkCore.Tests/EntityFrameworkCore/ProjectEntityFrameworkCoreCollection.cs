using Xunit;

namespace Abp.Dummy.Project.EntityFrameworkCore;

[CollectionDefinition(ProjectTestConsts.CollectionDefinitionName)]
public class ProjectEntityFrameworkCoreCollection : ICollectionFixture<ProjectEntityFrameworkCoreFixture>
{

}
