using TACPN;

namespace TapaalParser.TapaalGui.Placable;

public interface IPositionPlacementStrategy
{
    PlacableComponent FindLocationsFor(PetriNetComponent component);
}