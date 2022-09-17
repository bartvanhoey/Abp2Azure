using Abp2Azure.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Abp2Azure.Blazor;

public abstract class Abp2AzureComponentBase : AbpComponentBase
{
    protected Abp2AzureComponentBase()
    {
        LocalizationResource = typeof(Abp2AzureResource);
    }
}
