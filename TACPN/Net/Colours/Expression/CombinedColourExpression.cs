using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Expression;

public class CombinedColourExpression : IColourExpression
{
    public CombinedColourExpression(IEnumerable<ColourExpression> subExpressions, ColourType colourType)
    {
        var colourExpressions = subExpressions as ColourExpression[] ?? subExpressions.ToArray();
        GuardFrom.InvalidExpressionAssignment(colourExpressions, colourType);
        Colours = colourExpressions.Select(e => e.Colour);
        ColourType = colourType;
        
        if (colourExpressions.Count() > 1)
        {
            var exprs =  colourExpressions.Select(e=>$"{e.Amount}'{e.Expression}");
            Expression = exprs.Aggregate("", (first, second) => $"{first} + {second}");
        }
        else
        {
            var expression =  colourExpressions.First();
            Expression = $"{expression.Amount}'{expression.Colour}";
        }
    }

    public ColourType ColourType { get; init; }

    public IEnumerable<IColourExpressionEvaluatable> Colours { get;}

    public string Expression { get; }
}