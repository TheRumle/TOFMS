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
        if (colourExpressions.Length <= 1)
        {
            var expression = colourExpressions.First();
            return $"{expression.Amount}'{expression.ExpressionText}";
        }

        var exprs = colourExpressions.Select(e => $"{e.Amount}'{e.ExpressionText}");
        return string.Join(" + ", exprs);
    }

    public ColourType ColourType { get; init; }

    public IEnumerable<IColourValue> Colours { get;}

    public string ExpressionText { get; }
    public int Amount { get; }
    public IEnumerable<IColourExpression> AsAtomicValues()
    {
        return _subexpressions;
    }
}