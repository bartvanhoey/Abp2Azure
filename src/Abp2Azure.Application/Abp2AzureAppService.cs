using System;
using System.Collections.Generic;
using System.Text;
using Abp2Azure.Localization;
using Volo.Abp.Application.Services;

namespace Abp2Azure;

/* Inherit your application services from this class.
 */
public abstract class Abp2AzureAppService : ApplicationService
{
    protected Abp2AzureAppService()
    {
        LocalizationResource = typeof(Abp2AzureResource);
    }
}
