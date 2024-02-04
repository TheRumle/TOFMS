using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Exceptions;
using TACPN.Transitions.Guard.ColourComparison;

namespace TACPN.Transitions.Guard;

/// <summary>
/// Represents a series of OR conditions over variables where one must be true.
/// </summary>
public class TransitionGuardStatement : ITransitionGuardStatement
{
    internal TransitionGuardStatement(ColourType colourType)
    {
        ColourType = colourType;
    }

    public ColourType ColourType { get; }

    private ICollection<IColourComparison<ColourVariable, int>> _conditions = [];
    public IEnumerable<IColourComparison<ColourVariable, int>> Conditions => _conditions;

    public void AddComparison(IColourComparison<ColourVariable, int> comparator)
    {
        var lhs = comparator.Lhs;
        ColourType.MustContain(lhs);
        _conditions.Add(comparator);
    }

    public string ToTapaalText()
    {
        //AddOnlyTheComparisson
        //todo build a nice string expression (((x = 2 or .... s) and ((...))
        throw new NotImplementedException();
    }

    public static TransitionGuardStatement Empty(ColourType colourType) => new(colourType);

    public static TransitionGuardStatement WithConditions(ICollection<IColourComparison<ColourVariable, int>> conditions)
    {
        return new TransitionGuardStatement(ColourType.TokenAndDefaultColourType)
        {
            _conditions = conditions
        };
    }
}