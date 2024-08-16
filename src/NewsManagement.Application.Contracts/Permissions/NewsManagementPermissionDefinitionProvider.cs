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

    #region ListableContent
    var listableContentsPermission = newsManagement.AddPermission(NewsManagementPermissions.Categories.Default, L("Permission:ListableContents"));
    listableContentsPermission.AddChild(NewsManagementPermissions.ListableContents.Create, L("Permission:ListableContents.Create"));
    listableContentsPermission.AddChild(NewsManagementPermissions.ListableContents.Edit, L("Permission:ListableContents.Edit"));
    listableContentsPermission.AddChild(NewsManagementPermissions.ListableContents.Delete, L("Permission:ListableContents.Delete"));
    #endregion

    #region Video
    var videosPermission = newsManagement.AddPermission(NewsManagementPermissions.Categories.Default, L("Permission:Videos"));
    videosPermission.AddChild(NewsManagementPermissions.Videos.Create, L("Permission:Videos.Create"));
    videosPermission.AddChild(NewsManagementPermissions.Videos.Edit, L("Permission:Videos.Edit"));
    videosPermission.AddChild(NewsManagementPermissions.Videos.Delete, L("Permission:Videos.Delete"));
    #endregion

    #region News
    var newsesPermission = newsManagement.AddPermission(NewsManagementPermissions.Categories.Default, L("Permission:Newses"));
    newsesPermission.AddChild(NewsManagementPermissions.Categories.Create, L("Permission:Newses.Create"));
    newsesPermission.AddChild(NewsManagementPermissions.Categories.Edit, L("Permission:Newses.Edit"));
    newsesPermission.AddChild(NewsManagementPermissions.Categories.Delete, L("Permission:Newses.Delete"));
    #endregion

    #region Gallery
    var galleriesPermission = newsManagement.AddPermission(NewsManagementPermissions.Categories.Default, L("Permission:Galleries"));
    galleriesPermission.AddChild(NewsManagementPermissions.Galleries.Create, L("Permission:Galleries.Create"));
    galleriesPermission.AddChild(NewsManagementPermissions.Galleries.Edit, L("Permission:Galleries.Edit"));
    galleriesPermission.AddChild(NewsManagementPermissions.Galleries.Delete, L("Permission:Galleries.Delete"));
    #endregion
    


  }

  private static LocalizableString L(string name)
  {
    return LocalizableString.Create<NewsManagementResource>(name);
  }
}
