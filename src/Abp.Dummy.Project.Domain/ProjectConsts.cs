using Volo.Abp.Identity;

namespace Abp.Dummy.Project;

public static class ProjectConsts
{
    public const string DbTablePrefix = "App";
    public const string? DbSchema = null;
    public const string AdminEmailDefaultValue = IdentityDataSeedContributor.AdminEmailDefaultValue;
    public const string AdminPasswordDefaultValue = "1q2w3E*";
}
