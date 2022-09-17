using Abp2Azure.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Abp2Azure.Permissions;

public class Abp2AzurePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(Abp2AzurePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(Abp2AzurePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<Abp2AzureResource>(name);
    }
}
