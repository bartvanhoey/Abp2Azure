using Abp2Azure.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Abp2Azure.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class Abp2AzureController : AbpControllerBase
{
    protected Abp2AzureController()
    {
        LocalizationResource = typeof(Abp2AzureResource);
    }
}
