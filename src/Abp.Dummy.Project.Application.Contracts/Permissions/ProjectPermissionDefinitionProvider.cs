using Abp.Dummy.Project.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Abp.Dummy.Project.Permissions;

public class ProjectPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ProjectPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(ProjectPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(ProjectPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(ProjectPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(ProjectPermissions.Books.Delete, L("Permission:Books.Delete"));

        var authorsPermission = myGroup.AddPermission(ProjectPermissions.Authors.Default, L("Permission:Authors"));
        authorsPermission.AddChild(ProjectPermissions.Authors.Create, L("Permission:Authors.Create"));
        authorsPermission.AddChild(ProjectPermissions.Authors.Edit, L("Permission:Authors.Edit"));
        authorsPermission.AddChild(ProjectPermissions.Authors.Delete, L("Permission:Authors.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ProjectPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ProjectResource>(name);
    }
}
