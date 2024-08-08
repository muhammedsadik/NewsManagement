using NewsManagement.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace NewsManagement.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(NewsManagementEntityFrameworkCoreModule),
    typeof(NewsManagementApplicationContractsModule)
    )]
public class NewsManagementDbMigratorModule : AbpModule
{

}
