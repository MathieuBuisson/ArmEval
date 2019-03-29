using ArmEval.Core.ArmTemplate;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Newtonsoft.Json.Linq;

namespace ArmEval.Core.ArmClient
{
    public interface IArmDeployment
    {
        IResourceManagementClient Client { get; set; }
        Deployment Deployment { get; set; }
        string DeploymentName { get; }
        ResourceGroup ResourceGroup { get; set; }
        JObject Template { get; set; }

        string Invoke();
    }
}