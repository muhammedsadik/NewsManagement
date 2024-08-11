using NewsManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NewsManagement.Permissions;

public class NewsManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
  public override void Define(IPermissionDefinitionContext context)
  {
    var newsManagement = context.AddGroup(NewsManagementPermissions.GroupName);

    #region Tag
    var tagsPermission = newsManagement.AddPermission(NewsManagementPermissions.Tags.Default, L("Permission:Tags"));
    tagsPermission.AddChild(NewsManagementPermissions.Tags.Create, L("Permission:Tags.Create"));
    tagsPermission.AddChild(NewsManagementPermissions.Tags.Edit, L("Permission:Tags.Edit"));
    tagsPermission.AddChild(NewsManagementPermissions.Tags.Delete, L("Permission:Tags.Delete"));
    #endregion
    
    #region City
    var citysPermission = newsManagement.AddPermission(NewsManagementPermissions.Cities.Default, L("Permission:Cities"));
    citysPermission.AddChild(NewsManagementPermissions.Cities.Create, L("Permission:Cities.Create"));
    citysPermission.AddChild(NewsManagementPermissions.Cities.Edit, L("Permission:Cities.Edit"));
    citysPermission.AddChild(NewsManagementPermissions.Cities.Delete, L("Permission:Cities.Delete"));
    #endregion
    
    #region Category
    var categoriesPermission = newsManagement.AddPermission(NewsManagementPermissions.Categories.Default, L("Permission:Categories"));
    categoriesPermission.AddChild(NewsManagementPermissions.Categories.Create, L("Permission:Categories.Create"));
    categoriesPermission.AddChild(NewsManagementPermissions.Categories.Edit, L("Permission:Categories.Edit"));
    categoriesPermission.AddChild(NewsManagementPermissions.Categories.Delete, L("Permission:Categories.Delete"));
    #endregion





  }

  private static LocalizableString L(string name)
  {
    return LocalizableString.Create<NewsManagementResource>(name);
  }
}
