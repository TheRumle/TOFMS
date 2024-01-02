using TACPN.Net.Colours.Evaluatable;
using TACPN.Net.Colours.Type;

namespace TACPN.Net.Transitions;

public class TransitionGuard//TODO this needs to be conjunction or disjunction
{
    public TransitionGuard(ColourType colourType)
    {
        ColourType = colourType;
    }

    public ColourType ColourType { get; }
    private List<ColourComparison> _colourComparisons = new List<ColourComparison>();
    public IReadOnlyCollection<ColourComparison> Predicates => _colourComparisons.AsReadOnly();

    public void AddComparison(Colour lhs, BooleanOperator comparator, Colour rhs)
    {
        if (!ColourType.Colours.Contains(lhs)) throw new ArgumentException($"{lhs} is not in colour type {this.ColourType}");
        if (!ColourType.Colours.Contains(rhs)) throw new ArgumentException($"{rhs} is not in colour type {this.ColourType}");
        this._colourComparisons.Add(new ColourComparison(lhs, comparator, rhs));
    }
    
    public void AddComparison(ColourComparison comparator)
    {
        var lhs = comparator.Lhs;
        var rhs = comparator.Rhs;
        if (!ColourType.Colours.Contains(lhs)) throw new ArgumentException($"{lhs} is not in colour type {this.ColourType}");
        if (!ColourType.Colours.Contains(rhs)) throw new ArgumentException($"{rhs} is not in colour type {this.ColourType}");
        this._colourComparisons.Add(comparator);
    }
    public void AddComparison(IEnumerable<ColourComparison> comparisons)
    {
        foreach (var comparison in comparisons) AddComparison(comparison);
    }

    public static TransitionGuard Empty(ColourType colourType) => new(colourType);

}