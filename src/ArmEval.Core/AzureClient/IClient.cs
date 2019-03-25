using Microsoft.Azure.Management.ResourceManager;

namespace ArmEval.Core.AzureClient
{
    public interface IClient
    {
        IResourceManagementClient AzureClient { get; set; }
        string SubscriptionId { get; set; }
    }
}