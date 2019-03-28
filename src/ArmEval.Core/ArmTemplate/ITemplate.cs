using System.Collections.Generic;
using ArmEval.Core.UserInputs;

namespace ArmEval.Core.ArmTemplate
{
    public interface ITemplate
    {
        string ContentVersion { get; set; }
        IDictionary<string, object> Outputs { get; set; }
        IDictionary<string, object> Parameters { get; set; }
        IEnumerable<object> Resources { get; set; }
        string Schema { get; set; }
        IDictionary<string, object> Variables { get; set; }

        void AddExpression(ArmTemplateExpression expression, ArmValueTypes expectedOutputType);
        void AddExpression(ArmTemplateExpression expression, ArmValueTypes expectedOutputType, ICollection<ArmTemplateVariable> inputVariables);
        void AddInputParameters(ICollection<ArmTemplateParameter> inputParameters);
        void AddInputVariables(ICollection<ArmTemplateVariable> inputVariables);
    }
}