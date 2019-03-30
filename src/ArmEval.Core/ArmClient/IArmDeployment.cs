using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;

namespace ArmEval.Core.ArmClient
{
    public interface IArmDeployment
    {
        IResourceManagementClient Client { get; set; }
        Deployment Deployment { get; set; }
        string DeploymentName { get; }
        ResourceGroup ResourceGroup { get; set; }

        string Invoke();
    }
}