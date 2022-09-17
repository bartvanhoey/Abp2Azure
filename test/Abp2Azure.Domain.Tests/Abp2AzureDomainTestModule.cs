using Abp2Azure.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Abp2Azure;

[DependsOn(
    typeof(Abp2AzureEntityFrameworkCoreTestModule)
    )]
public class Abp2AzureDomainTestModule : AbpModule
{

}
