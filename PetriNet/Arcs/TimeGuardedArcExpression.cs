using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Colours.Values;

namespace TACPN.Arcs;

public class TimeGuardedArcExpression : ArcExpression
{
    public IList<ColourTimeGuard> TimeGuards { get;  }

    public TimeGuardedArcExpression(IEnumerable<ColourTimeGuard> timeGuards,  IEnumerable<IColourExpressionAmount> amounts, ColourType colourType) :
        base(amounts, colourType)
    {
        ColourTimeGuard[] colourTimeGuards = timeGuards as ColourTimeGuard[] ?? timeGuards.ToArray();
        this.TimeGuards = colourTimeGuards;
    }
    
    
    public static TimeGuardedArcExpression CapacityExpression(int amount)
    {
        return new TimeGuardedArcExpression(new[] { ColourTimeGuard.CapacityGuard() },
            new[] { ColourExpression.CapacityExpression(amount) }, ColourType.DefaultColorType);
    }


}