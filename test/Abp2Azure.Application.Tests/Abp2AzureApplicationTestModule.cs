using Volo.Abp.Modularity;

namespace Abp2Azure;

[DependsOn(
    typeof(Abp2AzureApplicationModule),
    typeof(Abp2AzureDomainTestModule)
    )]
public class Abp2AzureApplicationTestModule : AbpModule
{

}
