using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Abp2Azure.Blazor;

[Dependency(ReplaceServices = true)]
public class Abp2AzureBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Abp2Azure";
}
