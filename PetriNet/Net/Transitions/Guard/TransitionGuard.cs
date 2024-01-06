using TACPN.Net.Colours;
using TACPN.Net.Colours.Type;
using TACPN.Net.Colours.Values;
using TACPN.Net.Exceptions;

namespace TACPN.Net.Transitions.Guard;

public class TransitionGuard//TODO this needs to be conjunction or disjunction
{
    private TransitionGuard(ColourType colourType)
    {
        ColourType = colourType;
    }

    public ColourType ColourType { get; }
    private List<ColourComparison> _colourComparisons = new();
    public IReadOnlyCollection<ColourComparison> Predicates => _colourComparisons.AsReadOnly();
    
    
    public void AddComparison<T>(ColourComparison<T> comparator) where T : Colour
    {
        var lhs = comparator.Lhs;
        var rhs = comparator.Rhs;
        ColourType.MustContain(lhs);
        ColourType.MustContain(rhs);
        _colourComparisons.Add(comparator);
    }


    public static TransitionGuard Empty(ColourType colourType) => new(colourType);

}