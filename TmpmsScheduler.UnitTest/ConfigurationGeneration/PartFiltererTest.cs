using System;
using System.Collections.Generic;
using FluentAssertions;
using JetBrains.Annotations;
using TestDataGenerator;
using Tmpms;
using TmpmsChecker.ConfigurationGeneration;
using TmpmsScheduler.UnitTest.ConfigurationGeneration.Execution;
using Xunit;

namespace TmpmsScheduler.UnitTest.ConfigurationGeneration;

[TestSubject(typeof(PartFilterer))]
public class PartFiltererTest : SinglePartMoveTest
{
    private const string PARTTYPE = "P1";
    private readonly LocationGenerator _locationGenerator = new([PARTTYPE]);
    
    [Theory]
    [InlineData(0,0,1)]
    [InlineData(0,1,2)]
    [InlineData(0,2,2)]
    public void WhenAgeIsBetweenMinMax_ShouldBeIncluded(int min, int age, int max)
    {
        HashSet<Invariant> invariant = [new Invariant(PARTTYPE, min, max)];
        
        var from = _locationGenerator.GetRegular() with { Invariants = invariant };
        var to = _locationGenerator.GetRegular();

        PartGenerator partGenerator = new PartGenerator([PARTTYPE], min, max);
        var target = partGenerator.GenerateSingle(age);
        IEnumerable<Part> parts = [target];

        var move = CreateMoveAction(PARTTYPE, 1, from, to, [], []);
        parts.RelevantFor(move).Should().Contain(target);
    }
    
    [Theory]
    [InlineData(0,0,2)]
    [InlineData(0,1,2)]
    public void When_MovingOutOfProcessingLocation_AndAgeIsNotMax_ShouldNotBeIncluded(int min, int age, int max)
    {
        HashSet<Invariant> invariant = [new Invariant(PARTTYPE, min, max)];
        
        var from = _locationGenerator.GetProcessing() with { Invariants = invariant };
        var to = _locationGenerator.GetRegular();

        PartGenerator partGenerator = new PartGenerator([PARTTYPE], min, max);
        var target = partGenerator.GenerateSingle(age);
        IEnumerable<Part> parts = [target];

        var move = CreateMoveAction(PARTTYPE, 1, from, to, [], []);
        parts.RelevantFor(move).Should().NotContain(target);
    }
    
    [Theory]
    [InlineData(0,2,2)]
    public void WhenJourneyIsNotTo_ShouldNotBeIncluded(int min, int age, int max)
    {
        HashSet<Invariant> invariant = [new Invariant(PARTTYPE, min, max)];
        var from = _locationGenerator.GetRegular() with { Invariants = invariant };
        var to = _locationGenerator.GetProcessing();

        PartGenerator partGenerator = new PartGenerator([PARTTYPE], min, max);
        var target = partGenerator.GenerateSingle(age, [_locationGenerator.GetProcessing()]);
        IEnumerable<Part> parts = [target];

        var move = CreateMoveAction(PARTTYPE, 1, from, to, [], []);
        parts.RelevantFor(move).Should().NotContain(target);
    }
    
    [Theory]
    [InlineData(0,0,1)]
    [InlineData(0,1,2)]
    [InlineData(0,2,2)]
    public void WhenJourneyIsTo_ShouldBeIncluded(int min, int age, int max)
    {
        HashSet<Invariant> invariant = [new Invariant(PARTTYPE, min, max)];
        
        var from = _locationGenerator.GetRegular() with { Invariants = invariant };
        var to = _locationGenerator.GetProcessing();

        PartGenerator partGenerator = new PartGenerator([PARTTYPE], min, max);
        var target = partGenerator.GenerateSingle(age, [to]);
        IEnumerable<Part> parts = [target];

        var move = CreateMoveAction(PARTTYPE, 1, from, to, [], []);
        parts.RelevantFor(move).Should().Contain(target);
    }
    
    
}