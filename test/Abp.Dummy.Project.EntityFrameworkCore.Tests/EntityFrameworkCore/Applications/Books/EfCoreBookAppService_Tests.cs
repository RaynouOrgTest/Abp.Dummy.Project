using Abp.Dummy.Project.Books;
using Xunit;

namespace Abp.Dummy.Project.EntityFrameworkCore.Applications.Books;

[Collection(ProjectTestConsts.CollectionDefinitionName)]
public class EfCoreBookAppService_Tests : BookAppService_Tests<ProjectEntityFrameworkCoreTestModule>
{

}