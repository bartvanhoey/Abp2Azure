using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Abp2Azure.Data;
using Volo.Abp.DependencyInjection;

namespace Abp2Azure.EntityFrameworkCore;

public class EntityFrameworkCoreAbp2AzureDbSchemaMigrator
    : IAbp2AzureDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreAbp2AzureDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the Abp2AzureDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<Abp2AzureDbContext>()
            .Database
            .MigrateAsync();
    }
}
