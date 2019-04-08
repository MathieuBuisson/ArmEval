namespace ArmEval.Core.UserInputs
{
    public class MissingInput : IMissingInput
    {
        public string Name { get; }
        public InputTypes InputType { get; }

        public MissingInput(string name, InputTypes inputType)
        {
            Name = name;
            InputType = inputType;
        }
    }
}
