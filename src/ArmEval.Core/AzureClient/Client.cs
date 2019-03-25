using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Management.ResourceManager;

namespace ArmEval.Core.AzureClient
{
    public class Client : IClient
    {
        public string SubscriptionId {
            get => AzureClient.SubscriptionId;
            set => AzureClient.SubscriptionId = value;
        }
        public IResourceManagementClient AzureClient { get; set; }

        public Client(IResourceManagementClient azureClient, string subscription)
        {
            AzureClient = azureClient;
            SubscriptionId = subscription;
        }
    }
}
