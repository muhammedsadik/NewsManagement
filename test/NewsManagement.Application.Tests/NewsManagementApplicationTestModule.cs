using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace NewsManagement;

[DependsOn(
    typeof(NewsManagementApplicationModule),
    typeof(NewsManagementDomainTestModule)
    )]
public class NewsManagementApplicationTestModule : AbpModule
{

}
