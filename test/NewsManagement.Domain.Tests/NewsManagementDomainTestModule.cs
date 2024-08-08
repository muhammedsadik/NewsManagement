using NewsManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NewsManagement;

[DependsOn(
    typeof(NewsManagementEntityFrameworkCoreTestModule)
    )]
public class NewsManagementDomainTestModule : AbpModule
{

}
