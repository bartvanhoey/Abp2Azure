using Abp2Azure.Samples;
using Xunit;

namespace Abp2Azure.EntityFrameworkCore.Domains;

[Collection(Abp2AzureTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<Abp2AzureEntityFrameworkCoreTestModule>
{

}
