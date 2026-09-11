using System.Threading.Tasks;

namespace Abp.Dummy.Project.Data;

public interface IProjectDbSchemaMigrator
{
    Task MigrateAsync();
}
