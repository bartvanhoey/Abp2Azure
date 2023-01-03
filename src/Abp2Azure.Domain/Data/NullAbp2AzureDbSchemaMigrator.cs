using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Abp2Azure.Data;

/* This is used if database provider does't define
 * IAbp2AzureDbSchemaMigrator implementation.
 */
public class NullAbp2AzureDbSchemaMigrator : IAbp2AzureDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
