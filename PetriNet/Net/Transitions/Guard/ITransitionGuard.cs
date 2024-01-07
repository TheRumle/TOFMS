using TACPN.Net.Colours.Type;
using TACPN.Net.Colours.Values;

namespace TACPN.Net.Transitions.Guard;

public interface ITransitionGuard
{
    ColourType ColourType { get; }
    IEnumerable<ColourComparison> Predicates { get; }
    void AddAndGuard<T>(ColourComparison<T> comparator) where T : Colour;
    void AddOrGuard<T>(ColourComparison<T> comparator) where T : Colour;
}