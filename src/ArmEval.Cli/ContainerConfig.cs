using ArmEval.Core.AzureClient;
using Autofac;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using System;
using System.Linq;
using System.Reflection;

namespace ArmEval.Cli
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables("ArmEval_")
                .Build();
            var creds = ApplicationTokenProvider.LoginSilentAsync(config["TenantId"], config["ClientId"], config["ClientSecret"]).Result;

            var builder = new ContainerBuilder();            
            builder.RegisterInstance(config).As<IConfigurationRoot>();
            builder.RegisterInstance(creds).As<ServiceClientCredentials>();
            builder.RegisterType<ResourceManagementClient>().As<IResourceManagementClient>();
            builder.RegisterType<Client>().As<IClient>();
            //builder.RegisterType<Application>().As<IApplication>();

            return builder.Build();
        }
    }
}
