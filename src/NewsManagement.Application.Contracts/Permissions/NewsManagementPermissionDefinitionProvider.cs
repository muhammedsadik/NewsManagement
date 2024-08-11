using NewsManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NewsManagement.Permissions;

public class NewsManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
  public override void Define(IPermissionDefinitionContext context)
  {
    var newsManagement = context.AddGroup(NewsManagementPermissions.GroupName);


    var tagsPermission = newsManagement.AddPermission(NewsManagementPermissions.Tags.Default, L("Permission:Tags"));
    tagsPermission.AddChild(NewsManagementPermissions.Tags.Create, L("Permission:Tags.Create"));
    tagsPermission.AddChild(NewsManagementPermissions.Tags.Edit, L("Permission:Tags.Edit"));
    tagsPermission.AddChild(NewsManagementPermissions.Tags.Delete, L("Permission:Tags.Delete"));

  }

  private static LocalizableString L(string name)
  {
    return LocalizableString.Create<NewsManagementResource>(name);
  }
}
