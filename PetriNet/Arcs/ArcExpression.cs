using TACPN.Colours.Expression;
using TACPN.Colours.Type;

namespace TACPN.Arcs;

public class ArcExpression : IColourExpression
{
    public ArcExpression(IEnumerable<IColourExpressionAmount> amounts, ColourType colourType)
    {
        var colourExpressionAmounts = amounts as IColourExpressionAmount[] ?? amounts.ToArray();
        Amounts = colourExpressionAmounts;

        ExpressionText = colourExpressionAmounts.Length > 1
            ? string.Join(" + ", colourExpressionAmounts.Select(e => $"{e.ExpressionText}")) 
            : colourExpressionAmounts.First().ExpressionText;
        
        ColourValue = new TupleColour(colourExpressionAmounts.Select(e=>e.ColourValue), colourType);
        ColourType = colourType;
    }
    
    public ArcExpression(IColourExpressionAmount amount,  ColourType colourType)
    {
        Amounts = new []{amount};
        ExpressionText = amount.ExpressionText;
        ColourValue = amount.ColourValue;
        this.ColourType = colourType;
    }
    public IEnumerable<IColourExpressionAmount> Amounts{ get; protected init; }
    public ColourType ColourType { get; }
    public string ExpressionText { get; }
    public IColourValue ColourValue { get; }
}