using NewsManagement.Localization;
using NewsManagement.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Features;
using Volo.Abp.Localization;

namespace NewsManagement
{
  public class NewsManagementDefinitionProvider : FeatureDefinitionProvider
  {
    public override void Define(IFeatureDefinitionContext context)
    {
      var newsGroup = context.AddGroup("NewsApp");

      newsGroup.AddFeature(
       MultiTenancyConsts.News,
       displayName: LocalizableString.Create<NewsManagementResource>("News")
      );

      newsGroup.AddFeature(
        MultiTenancyConsts.Video, 
        displayName: LocalizableString.Create<NewsManagementResource>("Video")
      );

      newsGroup.AddFeature(
       MultiTenancyConsts.Gallery,
       displayName: LocalizableString.Create<NewsManagementResource>("Gallery")
      );
      
      newsGroup.AddFeature(
       MultiTenancyConsts.ListableContent,
        displayName: LocalizableString.Create<NewsManagementResource>("ListableContent")
      );

    }
  }
}
