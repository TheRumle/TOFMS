using TACPN.Net.Colours.Expression;
using TACPN.Net.Places;
using TACPN.Net.Transitions;

namespace TACPN.Net.Arcs;

public class OutGoingArc : Arc<Transition, IPlace>
{
    public IColourExpression Expression { get; set; }

    public OutGoingArc(Transition from, IPlace to, IColourExpression colourExpression) : base(from, to)
    {
        GuardFrom.InvalidExpressionAssignment(from, to, colourExpression);
        Expression = colourExpression;
    }
}   