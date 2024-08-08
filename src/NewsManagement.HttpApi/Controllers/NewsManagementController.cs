using NewsManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NewsManagement.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class NewsManagementController : AbpControllerBase
{
    protected NewsManagementController()
    {
        LocalizationResource = typeof(NewsManagementResource);
    }
}
