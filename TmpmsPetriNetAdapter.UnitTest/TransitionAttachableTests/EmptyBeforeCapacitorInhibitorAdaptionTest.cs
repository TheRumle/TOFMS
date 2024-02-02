﻿using Common;
using FluentAssertions;
using FluentAssertions.Execution;
using TACPN.Transitions;
using Tmpms.Common;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace TmpmsPetriNetAdapter.UnitTest.TransitionAttachableTests;

public class EmptyBeforeCapacitorInhibitorAdaptionTest : TransitionAttacherTest
{
    protected HashSet<Location> EmptyBef =
    [
        new("MustBeEmpty", 10, new List<Invariant>
        {
            new(PartType, 0, Infteger.PositiveInfinity)
        }, true),

        new("MustBeEmptyToo", 10, new List<Invariant>(), true)
    ];
    
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldHaveCorrectWeightFromCorrectArcs(bool isProcessingLocation)
    {
        (Transition transition, _) = CreateAndAttach(isProcessingLocation);
        
        var emptyBeforeNames = EmptyBef.Select(l => l.Name);
        using (new AssertionScope())
        {
            transition.InhibitorArcs.Should().AllSatisfy(e => e.Weight.Should().Be(1));
            transition.InhibitorArcs.Should().AllSatisfy(e => emptyBeforeNames.Should().Contain(e.From.Name));
        }
    }
  
    public override ITransitionAttachable CreateFromLocation(Location location)
    {
        var journeys = GetJourneys(location);
        return new EmptyBeforeCapacitorInhibitorAdaption(EmptyBef, journeys.ToIndexedJourney());
    }
}