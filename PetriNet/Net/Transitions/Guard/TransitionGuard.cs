using TACPN.Net.Colours;
using TACPN.Net.Colours.Type;
using TACPN.Net.Colours.Values;
using TACPN.Net.Exceptions;

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
        ColourType.MustContain(lhs);
        ColourType.MustContain(rhs);
        _colourComparisons.Add(comparator);
    }
    
    public void AddComparison(ColourVariable lhs, ColourComparison comparator, int rhs)
    {
        ColourType.MustBeCompatibleWith(lhs);
        lhs.MustBeAssignableTo(rhs);
        _colourComparisons.Add(comparator);
    }
    
    public void AddComparison<T>(ColourComparison<T> comparator) where T : Colour
    {
        var lhs = comparator.Lhs;
        var rhs = comparator.Rhs;
        ColourType.MustContain(lhs);
        ColourType.MustContain(rhs);
        _colourComparisons.Add(comparator);
    }
    public void AddComparison<T>(IEnumerable<ColourComparison<T>> comparisons)  where T : Colour
    {
        foreach (var comparison in comparisons) AddComparison(comparison);
    }

    public static TransitionGuard Empty(ColourType colourType) => new(colourType);

}