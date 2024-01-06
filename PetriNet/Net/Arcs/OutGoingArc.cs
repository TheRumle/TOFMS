using TACPN.Net.Colours.Expression;
using TACPN.Net.Places;
using TACPN.Net.Transitions;

namespace TACPN.Net.Arcs;

public class OutGoingArc : Arc<Transition, IPlace>
{
    public ArcExpression Expression { get; set; }
    public IEnumerable<IColourExpressionAmount> Productions => Expression.Amounts;

    public OutGoingArc(Transition from, IPlace to, IColourExpressionAmount colourExpression) : base(from, to)
    {
        GuardFrom.InvalidExpressionAssignment(from, to, colourExpression);
        Expression = new ArcExpression(colourExpression);
    }
    
    public OutGoingArc(Transition from, IPlace to, IEnumerable<IColourExpressionAmount> colourExpression) : base(from, to)
    {
        foreach (var expression in colourExpression)
            GuardFrom.InvalidExpressionAssignment(from, to, expression);
        
        Expression = new ArcExpression(colourExpression);
    }
}   