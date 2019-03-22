namespace ArmEval.Core.UserInputs
{
    public interface IArmTemplateInput
    {
        string Name { get; }
        object Value { get; }
    }
}