using System.Threading.Tasks;

namespace NewsManagement.Data;

public interface INewsManagementDbSchemaMigrator
{
    Task MigrateAsync();
}
