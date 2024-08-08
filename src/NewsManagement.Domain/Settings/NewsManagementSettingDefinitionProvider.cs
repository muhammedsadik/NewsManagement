using Volo.Abp.Settings;

namespace NewsManagement.Settings;

public class NewsManagementSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(NewsManagementSettings.MySetting1));
    }
}
