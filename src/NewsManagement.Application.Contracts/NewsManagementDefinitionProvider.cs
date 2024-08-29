using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Features;

namespace NewsManagement
{
  public class NewsManagementDefinitionProvider : FeatureDefinitionProvider
  {
    public override void Define(IFeatureDefinitionContext context)
    {
      var newsGroup = context.AddGroup("NewsApp");

      newsGroup.AddFeature("NewsApp.Video", defaultValue: "false");
      newsGroup.AddFeature("NewsApp.ListableContent", defaultValue: "false");
    }
  }
}
