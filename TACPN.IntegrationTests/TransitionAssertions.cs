using FluentAssertions;
using FluentAssertions.Collections;
using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;

namespace TACPN.IntegrationTests;

public static class TransitionAssertions
{
    public static AndWhichConstraint<GenericCollectionAssertions<IngoingArc>, IngoingArc> ShouldHaveArcFrom(this Transition transition, Place from)
    {
        return transition.InGoing.Should().ContainSingle(e => e.From.Equals(from));
    }
    
    public static AndWhichConstraint<GenericCollectionAssertions<IngoingArc>, IngoingArc> ShouldHaveArcFrom(this Transition transition, string placeName)
    {
        return transition.InGoing.Should().ContainSingle(x => x.From.Name == placeName);
    }
    
    public static Transition ShouldHaveArcsFrom(this Transition transition, params string[] placeNames)
    {
        foreach (var placeName in placeNames)
            transition.GetArcFrom(placeName).Should().NotBeNull( $"Transition did not have an arc from {placeName}");

        return transition;
    }
    
    public static Transition ShouldHaveArcsFrom(this Transition transition, params Place[] places)
    {
        foreach (var place in places)
            transition.GetArcFrom(place).Should().NotBeNull( $"transition did not have an arc from {place.Name}");

        return transition;
    }
    
    public static Transition ShouldHaveArcsTo(this Transition transition, params string[] placeNames)
    {
        foreach (var placeName in placeNames)
            transition.GetArcFrom(placeName).Should().NotBeNull( $"transition did not have an arc from {placeName}");

        return transition;
    }
    
    public static Transition ShouldHaveArcsTo(this Transition transition, params Place[] places)
    {
        foreach (var place in places)
            transition.GetArcTo(place).Should().NotBeNull($"Transition did not have an arc from {place.Name}");

        return transition;
    }
}