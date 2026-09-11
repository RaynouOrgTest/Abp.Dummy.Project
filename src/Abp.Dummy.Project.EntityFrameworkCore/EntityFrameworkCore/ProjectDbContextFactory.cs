using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Abp.Dummy.Project.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class ProjectDbContextFactory : IDesignTimeDbContextFactory<ProjectDbContext>
{
    public ProjectDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        ProjectEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<ProjectDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new ProjectDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Abp.Dummy.Project.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
