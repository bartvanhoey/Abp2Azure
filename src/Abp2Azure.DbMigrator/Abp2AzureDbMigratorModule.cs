using Abp2Azure.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Abp2Azure.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(Abp2AzureEntityFrameworkCoreModule),
    typeof(Abp2AzureApplicationContractsModule)
    )]
public class Abp2AzureDbMigratorModule : AbpModule
{

}
