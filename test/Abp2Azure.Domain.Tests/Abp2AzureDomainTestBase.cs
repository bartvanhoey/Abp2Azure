using Volo.Abp.Modularity;

namespace Abp2Azure;

/* Inherit from this class for your domain layer tests. */
public abstract class Abp2AzureDomainTestBase<TStartupModule> : Abp2AzureTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
