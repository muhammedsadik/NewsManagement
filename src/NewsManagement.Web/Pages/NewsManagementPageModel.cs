using NewsManagement.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace NewsManagement.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class NewsManagementPageModel : AbpPageModel
{
    protected NewsManagementPageModel()
    {
        LocalizationResourceType = typeof(NewsManagementResource);
    }
}
