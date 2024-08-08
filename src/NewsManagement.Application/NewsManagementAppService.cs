using System;
using System.Collections.Generic;
using System.Text;
using NewsManagement.Localization;
using Volo.Abp.Application.Services;

namespace NewsManagement;

/* Inherit your application services from this class.
 */
public abstract class NewsManagementAppService : ApplicationService
{
    protected NewsManagementAppService()
    {
        LocalizationResource = typeof(NewsManagementResource);
    }
}
