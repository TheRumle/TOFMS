using TACPN.Net.Colours;
using TACPN.Net.Colours.Expression;
using TACPN.Net.Exceptions;
using TACPN.Net.Places;
using TACPN.Net.Transitions;

namespace TACPN.Net.Arcs;

public class OutGoingArc : Arc<Transition, IPlace>
{
    public ArcExpression Expression { get; private set; }
    public IEnumerable<IColourExpressionAmount> Productions => Expression.Amounts;

    public OutGoingArc(Transition from, IPlace to, IColourExpressionAmount colourExpression) : base(from, to)
    {
        ArcGuards.InvalidExpressionAssignment(from, to, colourExpression);
        Expression = new ArcExpression(colourExpression, to.ColourType);
    }
    
    public OutGoingArc(Transition from, IPlace to, IEnumerable<IColourExpressionAmount> colourExpression) : base(from, to)
    {
        foreach (var expression in colourExpression)
            ArcGuards.InvalidExpressionAssignment(from, to, expression);
        
        Expression = new ArcExpression(colourExpression, to.ColourType);
    }

    public void SubstituteArcExpressionFor(ColourExpression colourExpression)
    {
        Expression.ColourType.MustBeCompatibleWith(colourExpression.ColourValue);
        Expression = new ArcExpression(colourExpression, Expression.ColourType);
    }
}   