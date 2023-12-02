using TACPN.Net;
using TACPN.Net.Transitions;

namespace TapaalParser.TapaalGui.Placable.PlacementStrategies;

public class MessyStrategy : IPositionPlacementStrategy
{
    public PlacableComponent FindLocationsFor(PetriNetComponent component)
    {
        var transitions = component.Transitions.Select(e => new Placement<Transition>(e, new Position(0, 0)));
        var places = component.Places.Select(e => new Placement<Place>(e, new Position(0, 0)));
        return new PlacableComponent(transitions, places, component.Colors, component.Name);
    }
}