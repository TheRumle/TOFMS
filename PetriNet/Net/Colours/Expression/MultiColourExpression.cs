using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Expression;

public class MultiColourExpression : ICompositeColourExpression
{
    private readonly IEnumerable<ColourExpression> _subexpressions;

    public MultiColourExpression(IEnumerable<ColourExpression> subExpressions, ColourType colourType)
    {
        var colourExpressions = subExpressions as ColourExpression[] ?? subExpressions.ToArray();
        GuardFrom.InvalidExpressionAssignment(colourExpressions, colourType);
        Colours = colourExpressions.Select(e => e.Colour);
        ColourType = colourType;
        _subexpressions = colourExpressions;
        ExpressionText = string.Join(" + ", colourExpressions.Select(e => $"{e.ExpressionText}"));
    }

    public ColourType ColourType { get; init; }

    public IEnumerable<IColourValue> Colours { get;}

    public string ExpressionText { get; }
    public IEnumerable<IColourExpression> AsAtomicValues()
    {
        return _subexpressions;
    }
}