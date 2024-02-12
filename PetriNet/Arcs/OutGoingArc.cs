using TACPN.Colours.Expression;
using TACPN.Exceptions;
using TACPN.Places;
using TACPN.Transitions;

namespace TACPN.Arcs;

public class OutGoingArc : Arc<Transition, Place>
{
    public ArcExpression Expression { get; private set; }
    public IEnumerable<IColourExpressionAmount> Productions => Expression.Amounts;

    public OutGoingArc(Transition from, Place to, IColourExpressionAmount colourExpression) : base(from, to)
    {
        ArcGuards.InvalidExpressionAssignment(from, to, colourExpression);
        Expression = new ArcExpression(colourExpression, to.ColourType);
    }
    
    public OutGoingArc(Transition from, Place to, IEnumerable<IColourExpressionAmount> colourExpression) : base(from, to)
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