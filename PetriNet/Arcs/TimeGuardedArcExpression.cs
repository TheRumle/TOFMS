using TACPN.Colours.Expression;
using TACPN.Colours.Type;

namespace TACPN.Arcs;

public class TimeGuardedArcExpression : ArcExpression
{
    public IList<ColourTimeGuard> TimeGuards { get;  }

    public TimeGuardedArcExpression(IEnumerable<ColourTimeGuard> timeGuards,  IEnumerable<IColourExpressionAmount> amounts, ColourType colourType) :
        base(amounts, colourType)
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