using NewsManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NewsManagement.Permissions;

public class NewsManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(NewsManagementPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(NewsManagementPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<NewsManagementResource>(name);
    }
}
