using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Evaluatable;

public record VariableDecrement(string Variable, ColourType ColourType) : IColourExpressionEvaluatable
{
    public string Value { get; set; } = $"{Variable}--";
    public string Variable { get; init; } = Variable;

    public static implicit operator string(VariableDecrement color)
    {
        return color.Value;
    }
}