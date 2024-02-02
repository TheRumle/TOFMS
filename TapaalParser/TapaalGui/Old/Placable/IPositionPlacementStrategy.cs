using TACPN;

namespace TapaalParser.TapaalGui.Old.Placable;

public interface IPositionPlacementStrategy
{
    PlacableComponent FindLocationsFor(PetriNetComponent component);
}