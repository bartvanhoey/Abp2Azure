using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Abp2Azure;

[Dependency(ReplaceServices = true)]
public class Abp2AzureBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Abp2Azure";
}
