using TACPN.Net.Colours.Expression;

namespace TACPN.Net.Colours.Evaluatable;

public record ProductColour : IColourExpressionEvaluatable
{
    public ProductColour(IEnumerable<IColourExpressionEvaluatable> evaluatables)
    {
        var strings = evaluatables.Select(e => e.Value);
        Value = strings.Aggregate("", (first, second) => $"{first},{second}");
    }
    public string Value { get; }
}