using TACPN.Net.Colours.Expression;

namespace TACPN.Net.Arcs;

public class TimeGuardedArcExpression : ArcExpression
{
    public IList<ColourTimeGuard> TimeGuards { get;  }

    public TimeGuardedArcExpression(IEnumerable<ColourTimeGuard> timeGuards,  IEnumerable<IColourExpressionAmount> amounts) :
        base(amounts)
    {
        ColourTimeGuard[] colourTimeGuards = timeGuards as ColourTimeGuard[] ?? timeGuards.ToArray();
        var guardedExpressions = colourTimeGuards.Select(e => e.ColourValue);
        var colourExpressionAmounts = amounts as IColourExpressionAmount[] ?? amounts.ToArray();
        var consProdExpressions = colourExpressionAmounts.Select(e => e.ColourValue);
        if (colourTimeGuards.Count() != colourExpressionAmounts.Count() || 
            !consProdExpressions.All(e => guardedExpressions.Contains(e)))
            throw new ArgumentException("A consumption/production does not have a time guard!");

        this.TimeGuards = colourTimeGuards;
    }

}