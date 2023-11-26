using TACPN.Net;
using Tofms.Common;

namespace TapaalParser.TapaalGui.Placable;

public interface IPositionPlacementStrategy
{
    PlacableComponent FindLocationsFor(PetriNetComponent component);
}