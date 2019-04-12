
namespace ArmEval.Core.UserInputs
{
    public class ArmTemplateFunctionParameter : IArmTemplateInput
    {
        public string Name { get; }
        public object Value { get; }

        public ArmTemplateFunctionParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
