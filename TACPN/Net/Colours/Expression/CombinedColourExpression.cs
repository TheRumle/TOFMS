using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Expression;

public class CombinedColourExpression : ICompositeColourExpression
{
    private readonly IEnumerable<ColourExpression> _subexpressions;

    public CombinedColourExpression(IEnumerable<ColourExpression> subExpressions, ColourType colourType)
    {
        var colourExpressions = subExpressions as ColourExpression[] ?? subExpressions.ToArray();
        GuardFrom.InvalidExpressionAssignment(colourExpressions, colourType);
        Colours = colourExpressions.Select(e => e.Colour);
        ColourType = colourType;
        ExpressionText = AssignExpressionText(colourExpressions);
        this.Amount = colourExpressions.Sum(e => e.Amount);
        this._subexpressions = colourExpressions;
    }

    private static string AssignExpressionText(ColourExpression[] colourExpressions)
    {
        if (colourExpressions.Count() <= 1)
        {
            var expression = colourExpressions.First();
            return $"{expression.Amount}'{expression.Colour}";
        }

        var exprs = colourExpressions.Select(e => $"{e.Amount}'{e.ExpressionText}");
        return exprs.Aggregate("", (first, second) => $"{first} + {second}");
    }

    public ColourType ColourType { get; init; }

    public IEnumerable<IColourExpressionEvaluatable> Colours { get;}

    public string ExpressionText { get; set; }
    public int Amount { get; }
    public IEnumerable<IColourExpression> AsAtomicValues()
    {
        return _subexpressions;
    }
}