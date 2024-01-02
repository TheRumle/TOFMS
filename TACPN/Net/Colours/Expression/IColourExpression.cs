using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Expression;

public interface ICompositeColourExpression : IColourExpression
{
    public IEnumerable<IColourExpression> AsAtomicValues();
} 

public interface IColourExpression
{
    public ColourType ColourType { get; }
    public string ExpressionText { get; }
    public int Amount { get; }
}