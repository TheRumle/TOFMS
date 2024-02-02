using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Exceptions;
using TACPN.Places;
using TACPN.Transitions;

namespace TACPN.Arcs;

public class IngoingArc : Arc<IPlace, Transition>
{
    public ColourType ColourType { get; init; }
    public TimeGuardedArcExpression Expression { get; set;  }

    public IngoingArc(IPlace from, Transition to, IEnumerable<ColourTimeGuard> guards, IEnumerable<IColourExpressionAmount> expressions ) 
        : base(from, to)
    {
        ColourTimeGuard[] colourTimeGuards = guards as ColourTimeGuard[] ?? guards.ToArray();
        ArcGuards.InvalidArcColourAssignment(from, to);
        ArcGuards.InvalidGuardColourAssignment(from, to, colourTimeGuards);
        ColourType = from.ColourType;
        Expression = new TimeGuardedArcExpression(colourTimeGuards, expressions, from.ColourType);
    }

    public IList<ColourTimeGuard> Guards => Expression.TimeGuards;
    public bool TrySetGuardAmount(ColourTimeGuard guard, int amount)
    {
        var timeGuard = GetGuard(guard);
        if (timeGuard is null) return false;
        
        var consumption = Expression.Amounts.First(e => e.ColourValue == timeGuard.ColourValue);
        consumption.Amount = amount;
        return true;
    }

    private ColourTimeGuard? GetGuard(ColourTimeGuard guard)
    {
        var timeGuard = Expression
            .TimeGuards
            .FirstOrDefault(e => e.ColourType == guard.ColourType);
        return timeGuard;
    }

    public bool TryAdjustGuardTime(ColourTimeGuard guard, Interval interval)
    {
        var timeGuard = GetGuard(guard);
        if (timeGuard is null) return false;
        timeGuard.Interval = interval;
        return true;
    }
    
    public void SubstituteArcExpressionFor(TimeGuardedArcExpression colourExpression)
    {
        if (!colourExpression.ColourType.Equals(this.ColourType))
            throw new ArgumentException(
                $"Cannot substitute expression of colour {this.Expression.ColourType} with {colourExpression.ColourType}");
            Expression = colourExpression;
    }
    
}