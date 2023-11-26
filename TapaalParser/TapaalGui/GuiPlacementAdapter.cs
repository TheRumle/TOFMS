using TACPN.Net;
using TapaalParser.TapaalGui.Placable;
using TapaalParser.TapaalGui.Placable.PlacementStrategies;

namespace TapaalParser.TapaalGui;



public class GuiPlacementAdapter
{
    private readonly IPositionPlacementStrategy? _positionPlacementStrategy;
    private readonly PetriNetComponent _component;

    public GuiPlacementAdapter(PetriNetComponent component, IPositionPlacementStrategy strategy)
    {
        this._component = component;
        this._positionPlacementStrategy = strategy;
    }
    
    public GuiPlacementAdapter(PetriNetComponent component)
    {
        this._component = component;
    }

    private IPositionPlacementStrategy SelectStrategy(PetriNetComponent component)
    {
        if (_positionPlacementStrategy is not null) return _positionPlacementStrategy;
        if (component.Transitions.Count == 1) return new CircleAroundSingleTransitionStrategy(component.Transitions.First(), new Position(300,300), 200);
        return new MessyStrategy();
    }
    
    
    public PlacableComponent GetComponentPlacements(PetriNetComponent component)
    {
        var strategy = SelectStrategy(component);
        return strategy.FindLocationsFor(component);
    }
}