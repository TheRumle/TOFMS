using TACPN.Net.Colours.Type;
using TACPN.Net.Colours.Values;

namespace TACPN.Net.Transitions.Guard;

public class TransitionGuard//TODO this needs to be conjunction or disjunction
{
    public TransitionGuard(ColourType colourType)
    {
        ColourType = colourType;
    }

    public ColourType ColourType { get; }
    private List<ColourComparison> _colourComparisons = new();
    public IReadOnlyCollection<ColourComparison> Predicates => _colourComparisons.AsReadOnly();

    public void AddComparison(Colour lhs, ColourComparison comparator, Colour rhs)
    {
        if (!ColourType.Colours.Contains(lhs)) throw new ArgumentException($"{lhs} is not in colour type {ColourType}");
        if (!ColourType.Colours.Contains(rhs)) throw new ArgumentException($"{rhs} is not in colour type {ColourType}");
        this._colourComparisons.Add(comparator);
    }
    
    public void AddComparison(ColourVariable lhs, ColourComparison comparator, int rhs)
    {
        if (!ColourType.IsCompatibleWith(lhs)) throw new ArgumentException($"{lhs} is not in colour type {ColourType}");
        if (!lhs.VariableValues.AsInts.Contains(rhs)) throw new ArgumentException($"{rhs} is not in colour type {ColourType}");
        this._colourComparisons.Add(comparator);
    }
    
    public void AddComparison<T>(ColourComparison<T> comparator) where T : Colour
    {
        var lhs = comparator.Lhs;
        var rhs = comparator.Rhs;
        if (!ColourType.Colours.Contains(lhs)) throw new ArgumentException($"{lhs} is not in colour type {ColourType}");
        if (!ColourType.Colours.Contains(rhs)) throw new ArgumentException($"{rhs} is not in colour type {ColourType}");
        this._colourComparisons.Add(comparator);
    }
    public void AddComparison<T>(IEnumerable<ColourComparison<T>> comparisons)  where T : Colour
    {
        foreach (var comparison in comparisons) AddComparison(comparison);
    }

    public static TransitionGuard Empty(ColourType colourType) => new(colourType);

}