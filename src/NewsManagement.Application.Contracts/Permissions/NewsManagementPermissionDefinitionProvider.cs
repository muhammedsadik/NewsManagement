using NewsManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace NewsManagement.Permissions;

public class NewsManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
  public override void Define(IPermissionDefinitionContext context)
  {
    var newsManagement = context.AddGroup(NewsManagementPermissions.GroupName);

    #region Tag
    var tagsPermission = newsManagement.AddPermission(NewsManagementPermissions.Tags.Default, L("Permission:Tags"), multiTenancySide: MultiTenancySides.Host);
    tagsPermission.AddChild(NewsManagementPermissions.Tags.Create, L("Permission:Tags.Create"));// Main permissiona verdim tenant ile giriş yapınca nasıl davaranacağını görmek için.
    tagsPermission.AddChild(NewsManagementPermissions.Tags.Edit, L("Permission:Tags.Edit"));
    tagsPermission.AddChild(NewsManagementPermissions.Tags.Delete, L("Permission:Tags.Delete"));
    #endregion

    #region City
    var citysPermission = newsManagement.AddPermission(NewsManagementPermissions.Cities.Default, L("Permission:Cities"), multiTenancySide: MultiTenancySides.Host);
    citysPermission.AddChild(NewsManagementPermissions.Cities.Create, L("Permission:Cities.Create"), multiTenancySide: MultiTenancySides.Host);
    citysPermission.AddChild(NewsManagementPermissions.Cities.Edit, L("Permission:Cities.Edit"), multiTenancySide: MultiTenancySides.Host);
    citysPermission.AddChild(NewsManagementPermissions.Cities.Delete, L("Permission:Cities.Delete"), multiTenancySide: MultiTenancySides.Host);
    #endregion

    #region Category
    var categoriesPermission = newsManagement.AddPermission(NewsManagementPermissions.Categories.Default, L("Permission:Categories"), multiTenancySide: MultiTenancySides.Host);
    categoriesPermission.AddChild(NewsManagementPermissions.Categories.Create, L("Permission:Categories.Create"), multiTenancySide: MultiTenancySides.Host);
    categoriesPermission.AddChild(NewsManagementPermissions.Categories.Edit, L("Permission:Categories.Edit"), multiTenancySide: MultiTenancySides.Host);
    categoriesPermission.AddChild(NewsManagementPermissions.Categories.Delete, L("Permission:Categories.Delete"), multiTenancySide: MultiTenancySides.Host);
    #endregion

    #region Video 
    var videosPermission = newsManagement.AddPermission(NewsManagementPermissions.Videos.Default, L("Permission:Videos"));
    videosPermission.AddChild(NewsManagementPermissions.Videos.Create, L("Permission:Videos.Create"), multiTenancySide: MultiTenancySides.Host);
    videosPermission.AddChild(NewsManagementPermissions.Videos.Edit, L("Permission:Videos.Edit"), multiTenancySide: MultiTenancySides.Host);
    videosPermission.AddChild(NewsManagementPermissions.Videos.Delete, L("Permission:Videos.Delete"), multiTenancySide: MultiTenancySides.Host);
    #endregion

    #region News
    var newsesPermission = newsManagement.AddPermission(NewsManagementPermissions.Newses.Default, L("Permission:Newses"));
    newsesPermission.AddChild(NewsManagementPermissions.Newses.Create, L("Permission:Newses.Create"), multiTenancySide: MultiTenancySides.Host);
    newsesPermission.AddChild(NewsManagementPermissions.Newses.Edit, L("Permission:Newses.Edit"), multiTenancySide: MultiTenancySides.Host);
    newsesPermission.AddChild(NewsManagementPermissions.Newses.Delete, L("Permission:Newses.Delete"), multiTenancySide: MultiTenancySides.Host);
    #endregion

    #region Gallery
    var galleriesPermission = newsManagement.AddPermission(NewsManagementPermissions.Galleries.Default, L("Permission:Galleries"));
    galleriesPermission.AddChild(NewsManagementPermissions.Galleries.Create, L("Permission:Galleries.Create"), multiTenancySide: MultiTenancySides.Host);
    galleriesPermission.AddChild(NewsManagementPermissions.Galleries.Edit, L("Permission:Galleries.Edit"), multiTenancySide: MultiTenancySides.Host);
    galleriesPermission.AddChild(NewsManagementPermissions.Galleries.Delete, L("Permission:Galleries.Delete"), multiTenancySide: MultiTenancySides.Host);
    #endregion
    
    #region ListableContent
/*    var listableContentsPermission = newsManagement.AddPermission(NewsManagementPermissions.ListableContents.Default, L("Permission:ListableContents"));
    listableContentsPermission.AddChild(NewsManagementPermissions.ListableContents.Create, L("Permission:ListableContents.Create"));
    listableContentsPermission.AddChild(NewsManagementPermissions.ListableContents.Edit, L("Permission:ListableContents.Edit"));
    listableContentsPermission.AddChild(NewsManagementPermissions.ListableContents.Delete, L("Permission:ListableContents.Delete"));*/
    #endregion 


  }

  private static LocalizableString L(string name)
  {
    return LocalizableString.Create<NewsManagementResource>(name);
  }
}
