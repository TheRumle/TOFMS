using TACPN.Net.Colours.Type;
using TACPN.Net.Colours.Values;
using TACPN.Net.Exceptions;

namespace TACPN.Net.Transitions.Guard;

internal class TransitionGuard : ITransitionGuard
{
    internal TransitionGuard(ColourType colourType)
    {
        ColourType = colourType;
    }

    public ColourType ColourType { get; }
    private List<ICondition> _guards = new();
    public IEnumerable<ColourComparison> Predicates => _guards.Select(e=>e.Comparison);
    
    
    public void AddAndGuard<T>(ColourComparison<T> comparator) where T : Colour
    {
        var lhs = comparator.Lhs;
        var rhs = comparator.Rhs;
        ColourType.MustContain(lhs);
        ColourType.MustContain(rhs);
        _guards.Add(new AndCondition<T>(comparator));
    }
    
    public void AddOrGuard<T>(ColourComparison<T> comparator) where T : Colour
    {
        var lhs = comparator.Lhs;
        var rhs = comparator.Rhs;
        ColourType.MustContain(lhs);
        ColourType.MustContain(rhs);
        _guards.Add(new OrCondition<T>(comparator));
    }
    
    public static ITransitionGuard Empty(ColourType colourType) => new TransitionGuard(colourType);

    public override string ToString()
    {
        //todo build a nice string expression (((x = 2 or .... s) and ((...))
        return base.ToString();
    }
}