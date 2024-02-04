using Volo.Abp.Modularity;

namespace Abp2Azure;

public abstract class Abp2AzureApplicationTestBase<TStartupModule> : Abp2AzureTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
