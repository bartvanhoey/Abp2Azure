using Xunit;

namespace Abp2Azure.EntityFrameworkCore;

[CollectionDefinition(Abp2AzureTestConsts.CollectionDefinitionName)]
public class Abp2AzureEntityFrameworkCoreCollection : ICollectionFixture<Abp2AzureEntityFrameworkCoreFixture>
{

}
