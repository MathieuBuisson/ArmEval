using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;


namespace ArmEval.Core.Tests
{
    public class ArmTemplateBaseTests
    {
        [Fact]
        public void Constructor_InitializesArmTemplateBaseProperties()
        {
            var expectedSchema = @"https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#";
            var expectedContentVersion = "1.0.0.0";
            var actual = new ArmTemplateBase();

            Assert.Equal(expectedSchema, actual.Schema);
            Assert.Equal(expectedContentVersion, actual.ContentVersion);
            Assert.Equal(new Dictionary<string, object>(), actual.Parameters);
            Assert.Equal(new Dictionary<string, object>(), actual.Variables);
            Assert.Equal(new List<object>(), actual.Resources);
            Assert.Equal(new Dictionary<string, object>(), actual.Outputs);
        }

        [Fact]
        public void Deserialization_EmptyTemplateFile_ToArmTemplateBase()
        {
            var testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData/EmptyTemplate.json");
            var templateContent = File.ReadAllText(testFilePath);
            var actual = JsonConvert.DeserializeObject<ArmTemplateBase>(templateContent);

            Assert.IsType<ArmTemplateBase>(actual);
            Assert.False(string.IsNullOrWhiteSpace(actual.Schema));
            Assert.False(string.IsNullOrWhiteSpace(actual.ContentVersion));
            Assert.IsAssignableFrom<IDictionary<string, object>>(actual.Parameters);
            Assert.IsAssignableFrom<IDictionary<string, object>>(actual.Variables);
            Assert.IsAssignableFrom<IDictionary<string, object>>(actual.Outputs);
            Assert.IsType<List<object>>(actual.Resources);
        }

        [Fact]
        public void Deserialization_FullTemplateFile_ToArmTemplateBase()
        {
            var testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData/FullTemplate.json");
            var templateContent = File.ReadAllText(testFilePath);
            var expectedSchema = @"https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#";
            var expectedContentVersion = "1.0.0.0";
            var actual = JsonConvert.DeserializeObject<ArmTemplateBase>(templateContent);

            Assert.IsType<ArmTemplateBase>(actual);
            Assert.Equal(expectedSchema, actual.Schema);
            Assert.Equal(expectedContentVersion, actual.ContentVersion);

            Assert.IsAssignableFrom<IDictionary<string, object>>(actual.Parameters);
            Assert.Equal(12, actual.Parameters.Keys.Count);
            Assert.True(actual.Parameters.ContainsKey("clusterName"));
            Assert.True(actual.Parameters.ContainsKey("nodeCount"));

            Assert.IsAssignableFrom<IDictionary<string, object>>(actual.Variables);
            Assert.Equal(5, actual.Variables.Keys.Count);
            Assert.True(actual.Variables.ContainsKey("vnetName"));
            Assert.True(actual.Variables.ContainsKey("subnetName"));

            Assert.IsAssignableFrom<IDictionary<string, object>>(actual.Outputs);
            Assert.Equal(3, actual.Outputs.Keys.Count);
            Assert.True(actual.Outputs.ContainsKey("kubernetesMasterFqdn"));
            Assert.True(actual.Outputs.ContainsKey("aksResourceId"));

            Assert.IsType<List<object>>(actual.Resources);
            Assert.Equal(4, actual.Resources.Count);
            Assert.True(actual.Resources.All(r => r is Newtonsoft.Json.Linq.JObject));

        }
    }
}
