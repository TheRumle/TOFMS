﻿using FluentAssertions;
using FluentAssertions.Collections;
using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;

namespace TACPN.IntegrationTests;

public static class TransitionAssertions
{
    public static AndWhichConstraint<GenericCollectionAssertions<IngoingArc>, IngoingArc> ShouldHaveArcFrom(
        this Transition transition, Place from)
    {
        return transition.InGoing.Should().ContainSingle(e => e.From.Equals(from));
    }

    public static AndWhichConstraint<GenericCollectionAssertions<IngoingArc>, IngoingArc> ShouldHaveArcFrom(
        this Transition transition, string placeName)
    {
        return transition.InGoing.Should().ContainSingle(x => x.From.Name == placeName);
    }
    
    public static InhibitorArc? FindFirstInhibitorFromPlaceWithName(this Transition transition, params string[] nameParts)
    {
        var inhibitor = transition.InhibitorArcs.FirstOrDefault(e =>
        {
            var lowerName = e.From.Name.ToLower();
            return nameParts.All(part => lowerName.Contains(part.ToLower()));
        });
        return inhibitor;
    }

    public static Place FindFirstConnectedPlaceWithName(this Transition transition, params string[] nameParts)
    {
        var list = new List<IPlace>();
        list.AddRange(transition.InhibitorArcs.Select(e=>e.From));
        list.AddRange(transition.InGoing.Select(e=>e.From));
        list.AddRange(transition.OutGoing.Select(e=>e.To));

        var set = new HashSet<IPlace>(list);
        return set.OfType<Place>().First(e =>
        {
            var lowerName = e.Name.ToLower();
            return nameParts.All(part => lowerName.Contains(part.ToLower()));
        });
    }
    
    
    
    public static IngoingArc FindFirstIngoingFromPlaceContaining(this Transition transition, params string[] nameParts)
    {
        var ingoingArc = transition.InGoing.First(e =>
        {
            var lowerName = e.From.Name.ToLower();
            return nameParts.All(part => lowerName.Contains(part.ToLower()));
        });
        return ingoingArc;
    }
    
    public static OutGoingArc FindFirstOutgoingToPlaceWithName(this Transition transition, params string[] nameParts)
    {
        var outGoingArc = transition.OutGoing.First(e =>
        {
            var lowerName = e.To.Name.ToLower();
            return nameParts.All(part => lowerName.Contains(part.ToLower()));
        });
        return outGoingArc;
    }
    
    
    
}