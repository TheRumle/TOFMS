﻿using TACPN.Net;
using TACPN.Net.Transitions;
using Tofms.Common;

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
        var coordinates = PositionCalculator.GetCircularCoordinates(this._center.X, this._center.Y, this._radius, component.Places.Count);
        var placesArray = component.Places.ToList();
        var placePositions = GetPlacePlacements(component, coordinates, placesArray);
        var transitions = GetTransitionsPlacements(component, coordinates);
        return new PlacableComponent(transitions, placePositions, component.Colors);
    }

    private List<Placement<Transition>> GetTransitionsPlacements(PetriNetComponent component, Position[] coordinates)
    {
        var otherTransitionsPlacements = component.Transitions.Except(new[] { this._transition })
            .Select(e => new Placement<Transition>(e, Position.Zero));


        var transitions = new List<Placement<Transition>>(otherTransitionsPlacements)
        {
            new(_transition, PositionCalculator.FindMidPoint(coordinates))
        };
        return transitions;
    }

    private static List<Placement<Place>> GetPlacePlacements(PetriNetComponent component, Position[] coordinates, List<Place> placesArray)
    {
        List<Placement<Place>> placePositions = new List<Placement<Place>>(component.Places.Count);
        for (int i = 0; i < component.Places.Count; i++)
        {
            var position = coordinates[i];
            var place = placesArray[i];
            placePositions.Add(new Placement<Place>(place, position));
        }

        return placePositions;
    }
}