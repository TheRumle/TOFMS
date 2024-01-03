using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Evaluatable;

public record VariableDecrement : IColourTypedColourValue
{
    public VariableDecrement(Variable variable)
    {

        this.Variable = variable;
        this.ColourType = variable.ColourType;
        Value = $"{Variable.Value}--";
        this.VariableName = Variable.Name;
    }

    public Variable Variable { get; set; }

    public string Value { get; set; }
    public string VariableName { get; init; }
    public ColourType ColourType { get; init; }

    public override string ToString()
    {
        return this.Value;
    }
}