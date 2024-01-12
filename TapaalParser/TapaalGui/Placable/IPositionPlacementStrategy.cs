using TACPN;
using TACPN.Net;

namespace TapaalParser.TapaalGui.Placable;

public interface IPositionPlacementStrategy
{
    PlacableComponent FindLocationsFor(PetriNetComponent component);
}