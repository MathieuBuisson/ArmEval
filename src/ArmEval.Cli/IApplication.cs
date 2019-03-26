using ArmEval.Core.ArmTemplate;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;

namespace ArmEval.Cli
{
    public interface IApplication
    {
        string AzureRegion { get; }
        IResourceManagementClient Client { get; }
        ResourceGroup ResourceGroup { get; set; }
        ITemplate Template { get; set; }

        void Init();
    }
}