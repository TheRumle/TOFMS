using TACPN.Net.Colours.Expression;

namespace TACPN.Net.Arcs;

public class ArcExpression : IColourExpression
{
    public ArcExpression(IEnumerable<IColourExpressionAmount> amounts)
    {
        Amounts = amounts;
    }
    
    public ArcExpression(IColourExpressionAmount amount)
    {
        Amounts = new []{amount};
    }
    public IEnumerable<IColourExpressionAmount> Amounts{ get; protected init; }
}