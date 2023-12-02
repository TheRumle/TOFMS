using TACPN.Net;
using TapaalParser.TapaalGui.Placable;
using TapaalParser.TapaalGui.Placable.PlacementStrategies;

namespace TapaalParser.TapaalGui;



public class GuiPositioningFinder
{
    private readonly IPositionPlacementStrategy? _positionPlacementStrategy;
    private readonly PetriNetComponent _component;

    public GuiPositioningFinder(PetriNetComponent component, IPositionPlacementStrategy strategy)
    {
        this._component = component;
        this._positionPlacementStrategy = strategy;
    }
    
    public GuiPositioningFinder(PetriNetComponent component)
    {
        this._component = component;
    }

    private IPositionPlacementStrategy SelectStrategy(PetriNetComponent component)
    {
        if (_positionPlacementStrategy is not null) return _positionPlacementStrategy;
        if (component.Transitions.Count == 1) return new CircleAroundSingleTransitionStrategy(component.Transitions.First(), new Position(300,300), 200);
        return new MessyStrategy();
    }
    
    
    public PlacableComponent GetComponentPlacements()
    {
        var strategy = SelectStrategy(this._component);
        return strategy.FindLocationsFor(this._component);
    }
}