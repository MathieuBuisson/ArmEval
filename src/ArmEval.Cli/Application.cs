﻿using ArmEval.Core.Utils;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ArmEval.Cli
{
    public class Application : IApplication
    {
        readonly IConfigurationRoot _config;
        public IResourceManagementClient Client { get; private set; }
        public ResourceGroup ResourceGroup { get; set; }
        public string AzureRegion { get; private set; }
        
        public Application(IConfigurationRoot config, IResourceManagementClient client,
            ResourceGroup resourceGroup)
        {
            _config = config;
            Client = client;
            ResourceGroup = resourceGroup;
        }

        public void Init()
        {
            Client.SubscriptionId = _config["SubscriptionId"];
            AzureRegion = "North Europe";
            ResourceGroup.Location = AzureRegion;
            ResourceGroup.Name = $"ArmEval{UniqueString.Create(5)}";
        }
    }
}
