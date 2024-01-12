using TACPN.Colours.Type;

namespace TACPN.Colours.Expression;

public interface IColourExpression
{
    public ColourType ColourType { get; }
    public string ExpressionText { get; }
    public IColourValue ColourValue { get; }
}