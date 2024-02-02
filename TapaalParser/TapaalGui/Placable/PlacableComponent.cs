using TACPN.Places;
using TACPN.Transitions;

namespace TapaalParser.TapaalGui.Placable;

public record PlacableComponent(IEnumerable<Placement<Transition>> Transitions,
    IEnumerable<Placement<Place>> Places,
    IEnumerable<Placement<CapacityPlace>> CapacityPlaces, IEnumerable<string> Colors, string Name);