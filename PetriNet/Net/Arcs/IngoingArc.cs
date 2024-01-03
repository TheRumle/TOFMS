using TACPN.Net.Colours;
using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;
using TACPN.Net.Places;
using TACPN.Net.Transitions;

namespace TACPN.Net.Arcs;

public class IngoingArc : Arc<IPlace, Transition>
{
    public ColourType ColourType { get; init; }
    public IColourExpression Expression { get; set; }
    
    public IngoingArc(IPlace from, Transition to, IEnumerable<ColoredGuard> guards, IColourExpression expression ) : base(from, to)
    {
        var coloredGuards = guards as ColoredGuard[] ?? guards.ToArray();
        GuardFrom.InvalidArcColourAssignment(from, to);
        GuardFrom.InvalidGuardColourAssignment(from, to, coloredGuards);
        GuardFrom.InvalidExpressionAssignment(from, to, expression);
        ColourType = from.ColourType;
        Guards = coloredGuards.ToList();
        Expression = expression;
    }

    public IList<ColoredGuard> Guards { get; set; }
    public bool ReplaceGuardedExpression(ColoredGuard newGuard, ColourExpression expression)
    {
        var oldGuard = Guards.FirstOrDefault(e => e.ColourType == newGuard.ColourType);
        if (oldGuard is null) return false;
        this.Expression = expression;
        Guards.Remove(oldGuard);
        Guards.Add(newGuard);
        return true;
    }
    
    
}