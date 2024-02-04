using Volo.Abp.Modularity;

namespace Abp2Azure;

[DependsOn(
    typeof(Abp2AzureDomainModule),
    typeof(Abp2AzureTestBaseModule)
)]
public class Abp2AzureDomainTestModule : AbpModule
{

}
