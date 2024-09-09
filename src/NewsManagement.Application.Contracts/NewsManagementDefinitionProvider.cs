using NewsManagement.Localization;
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
        "NewsApp.Video", 
        defaultValue: "false", 
        displayName: LocalizableString.Create<NewsManagementResource>("Video")
      );

      newsGroup.AddFeature(
        "NewsApp.ListableContent",
        defaultValue: "false",
        displayName: LocalizableString.Create<NewsManagementResource>("ListableContent")
      );

      newsGroup.AddFeature(
       "NewsApp.Gallery",
       defaultValue: "false",
       displayName: LocalizableString.Create<NewsManagementResource>("Gallery")
      );


    }
  }
}
