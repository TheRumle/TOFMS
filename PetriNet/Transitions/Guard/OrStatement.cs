using System.Reflection.Metadata.Ecma335;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Exceptions;
using TACPN.Transitions.Guard.ColourComparison;

namespace TACPN.Transitions.Guard;

/// <summary>
/// Represents a series of OR conditions over variables where one must be true.
/// </summary>
public class OrStatement : IOrStatement
{
    public override string ToString()
    {
        return this.ToTapaalText();
    }

    internal OrStatement(ColourType colourType)
    {
        ColourType = colourType;
    }

    public ColourType ColourType { get; }

    private ICollection<IColourComparison<ColourVariable, int>> _comparisons = [];
    public IEnumerable<IColourComparison<ColourVariable, int>> Comparisons => _comparisons;

    public void AddComparison(IColourComparison<ColourVariable, int> comparator)
    {
        var lhs = comparator.Lhs;
        ColourType.MustContain(lhs);
        _comparisons.Add(comparator);
    }

    public string ToTapaalText()
    {
        if (Comparisons.Count() == 1) return $"{_comparisons.First()}";
        var result = String.Concat(Enumerable.Repeat('(', Comparisons.Count()-1));

        var startBlock = Comparisons.First().ToString() + " OR ";
        
        result += Comparisons.Skip(1).Take(Comparisons.Count()-2).Aggregate(startBlock, (previous, comparison) =>
        {
            previous += comparison +")" + " OR ";
            return previous;
        });
        
        result += Comparisons.Last() + ")";
        return result;
    }

    public static OrStatement Empty(ColourType colourType) => new(colourType);

    public static OrStatement WithConditions(ICollection<IColourComparison<ColourVariable, int>> conditions)
    {
        return new OrStatement(ColourType.TokenAndDefaultColourType)
        {
            _comparisons = conditions
        };
    }
}