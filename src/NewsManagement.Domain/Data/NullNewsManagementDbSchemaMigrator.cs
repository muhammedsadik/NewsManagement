using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace NewsManagement.Data;

/* This is used if database provider does't define
 * INewsManagementDbSchemaMigrator implementation.
 */
public class NullNewsManagementDbSchemaMigrator : INewsManagementDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
