using Abp2Azure.Samples;
using Xunit;

namespace Abp2Azure.EntityFrameworkCore.Applications;

[Collection(Abp2AzureTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<Abp2AzureEntityFrameworkCoreTestModule>
{

}
