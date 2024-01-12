using TACPN.Colours.Expression;

namespace TACPN.Colours.Values;

public interface IColourVariableExpression : IColourTypedValue
{
    public ColourVariable ColourVariable { get; }
}