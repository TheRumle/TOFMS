using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Exceptions;
using TACPN.Transitions.Guard.ColourComparison;

namespace TACPN.Transitions.Guard;

/// <summary>
/// Represents a series of OR conditions over variables where one must be true.
/// </summary>
internal class TransitionGuardStatement : ITransitionGuardStatement
{
    internal TransitionGuardStatement(ColourType colourType)
    {
        ColourType = colourType;
    }

    public ColourType ColourType { get; }

    private List<IColourComparison<ColourVariable, int>> _conditions = new();

    public void AddComparison(IColourComparison<ColourVariable, int> comparator)
    {
        var lhs = comparator.Lhs;
        ColourType.MustContain(lhs);
        _conditions.Add(comparator);
    }
    
    public static TransitionGuardStatement Empty(ColourType colourType) => new(colourType);

    public override string ToString()
    {
        //todo build a nice string expression (((x = 2 or .... s) and ((...))
        return base.ToString();
    }
}