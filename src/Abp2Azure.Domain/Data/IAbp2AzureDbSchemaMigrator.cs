using System.Threading.Tasks;

namespace Abp2Azure.Data;

public interface IAbp2AzureDbSchemaMigrator
{
    Task MigrateAsync();
}
