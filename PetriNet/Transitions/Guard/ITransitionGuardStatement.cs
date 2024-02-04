using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Transitions.Guard.ColourComparison;

namespace TACPN.Transitions.Guard;


public interface ITransitionGuardStatement
{
    ColourType ColourType { get; }
    void AddComparison(IColourComparison<ColourVariable, int> comparator);
    public IEnumerable<IColourComparison<ColourVariable, int>> Conditions { get; }

    public string ToTapaalText();
}