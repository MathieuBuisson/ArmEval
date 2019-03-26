using ArmEval.Core.ArmTemplate;
using ArmEval.Core.Utils;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ArmEval.Cli
{
    public class Application : IApplication
    {
        IConfigurationRoot _config;
        public IResourceManagementClient Client { get; private set; }
        public ITemplate Template { get; set; }
        public string ResourceGroup { get; private set; }
        public string AzureRegion { get; private set; }
        
        public Application(IConfigurationRoot config, IResourceManagementClient client,
            ITemplate template)
        {
            _config = config;
            Client = client;
            Template = template;
        }

        public void Init()
        {
            Client.SubscriptionId = _config["SubscriptionId"];
            ResourceGroup = $"ArmEval{UniqueString.Create(5)}";
            AzureRegion = "North Europe";
        }
    }
}
