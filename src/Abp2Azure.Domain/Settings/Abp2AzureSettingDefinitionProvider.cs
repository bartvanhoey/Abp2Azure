using Volo.Abp.Settings;

namespace Abp2Azure.Settings;

public class Abp2AzureSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(Abp2AzureSettings.MySetting1));
    }
}
