using ArmEval.Core.ArmTemplate;
using Microsoft.Azure.Management.ResourceManager;

namespace ArmEval.Cli
{
    public interface IApplication
    {
        string AzureRegion { get; }
        IResourceManagementClient Client { get; }
        string ResourceGroup { get; }
        ITemplate Template { get; set; }

        void Init();
    }
}