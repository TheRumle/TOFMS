using TACPN;
using TACPN.Places;
using TACPN.Transitions;

namespace TapaalParser.TapaalGui.Placable.PlacementStrategies;

/// <summary>
/// Places positions in a circle.
/// </summary>
public class CircleAroundSingleTransitionStrategy : IPositionPlacementStrategy
{
    private readonly Transition _transition;
    private readonly Position _center;
    private readonly double _radius;

    public CircleAroundSingleTransitionStrategy(Transition transition, Position center, double radius)
    {
        _transition = transition;
        _center = center;
        _radius = radius;
    }
    
    
    public PlacableComponent FindLocationsFor(PetriNetComponent component)
    {
        var coordinates = PositionCalculator.GetCircularCoordinates(_center.X, _center.Y, _radius, component.AllPlaces().Count);
        var placesArray = component.AllPlaces().ToList();
        var placePositions = GetPlacePlacements(component, coordinates, placesArray);
        var transitions = GetTransitionsPlacements(component, coordinates);
        
        return new PlacableComponent(transitions, 
            placePositions.OfType<Placement<Place>>(),
            placePositions.OfType<Placement<CapacityPlace>>(),
            component.ColourTypes.Select(e=>e.Name), component.Name);
    }

    private List<Placement<Transition>> GetTransitionsPlacements(PetriNetComponent component, Position[] coordinates)
    {
        var otherTransitionsPlacements = component.Transitions.Except(new[] { _transition })
            .Select(e => new Placement<Transition>(e, Position.Zero));


        var transitions = new List<Placement<Transition>>(otherTransitionsPlacements)
        {
            new(_transition, PositionCalculator.FindMidPoint(coordinates))
        };
        return transitions;
    }

    private static List<Placement<IPlace>> GetPlacePlacements(PetriNetComponent component, Position[] coordinates, List<IPlace> placesArray)
    {
        List<Placement<IPlace>> placePositions = new List<Placement<IPlace>>(component.AllPlaces().Count);
        
        for (int i = 0; i < component.AllPlaces().Count; i++)
        {
            var position = coordinates[i];
            var place = placesArray[i];
            placePositions.Add(new Placement<IPlace>(place, position));
        }

        return placePositions;
    }
    
    
}