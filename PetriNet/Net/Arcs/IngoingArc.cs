using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;
using TACPN.Net.Places;
using TACPN.Net.Transitions;

namespace TACPN.Net.Arcs;

public class IngoingArc : Arc<IPlace, Transition>
{
    public ColourType ColourType { get; init; }
    public TimeGuardedArcExpression ArcExpression { get; }
    public IEnumerable<IColourExpressionAmount> Consumptions => ArcExpression.Amounts;
    
    public IngoingArc(IPlace from, Transition to, IEnumerable<ColourTimeGuard> guards, IEnumerable<IColourExpressionAmount> expressions ) 
        : base(from, to)
    {
        GuardFrom.InvalidArcColourAssignment(from, to);
        GuardFrom.InvalidGuardColourAssignment(from, to, guards);
        ColourType = from.ColourType;
        ArcExpression = new TimeGuardedArcExpression(guards, expressions);
    }

    public IList<ColourTimeGuard> Guards => ArcExpression.TimeGuards;
    public bool TrySetGuardAmount(ColourTimeGuard guard, int amount)
    {
        var timeGuard = GetGuard(guard);
        if (timeGuard is null) return false;
        
        var consumption = ArcExpression.Amounts.First(e => e.ColourValue == timeGuard.ColourValue);
        consumption.Amount = amount;
        return true;
    }

    private ColourTimeGuard? GetGuard(ColourTimeGuard guard)
    {
        var timeGuard = ArcExpression
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
    
    
}