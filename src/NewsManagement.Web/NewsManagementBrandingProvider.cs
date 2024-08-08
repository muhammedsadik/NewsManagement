using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace NewsManagement.Web;

[Dependency(ReplaceServices = true)]
public class NewsManagementBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "NewsManagement";
}
