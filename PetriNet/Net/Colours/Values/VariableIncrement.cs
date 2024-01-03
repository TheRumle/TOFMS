using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Values;

public record VariableIncrement(string Variable, ColourType ColourType) : IColourTypedColourValue
{
    public string Value { get; set; } = $"{Variable}--";
    public string Variable { get; init; } = Variable;

    public static implicit operator string(VariableIncrement color)
    {
        return color.Value;
    }
}