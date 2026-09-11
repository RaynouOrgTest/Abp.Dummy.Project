using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Abp.Dummy.Project.Data;
using Volo.Abp.DependencyInjection;

namespace Abp.Dummy.Project.EntityFrameworkCore;

public class EntityFrameworkCoreProjectDbSchemaMigrator
    : IProjectDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreProjectDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the ProjectDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ProjectDbContext>()
            .Database
            .MigrateAsync();
    }
}
