using TACPN.Net.Colours.Expression;

namespace TACPN.Net.Colours.Values;

public interface IColourVariableExpression : IColourTypedValue
{
    public ColourVariable ColourVariable { get; }
}