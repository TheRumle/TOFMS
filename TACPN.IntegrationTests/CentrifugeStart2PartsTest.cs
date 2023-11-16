using Common;
using Common.Move;
using FluentAssertions;
using FluentAssertions.Execution;
using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;
using Xunit;

namespace TACPN;



public class CentrifugeStart2PartsTest
{
    private readonly TofmToTacpnTranslater _translater = new();
    private static readonly Location BufferLocation = ExampleLocations.CBuffer;
    private static readonly Location ProcLocation = ExampleLocations.CProc;
    private static readonly int ProcMaxAge = 12;
    private const string PartName = "p1";
    private static readonly string CapacityColor = CapacityPlaceCreator.DefaultColorName;

    public MoveAction Move = MoveBuilder.BuildSinglePartTypeAction("CStart2P1", BufferLocation, ProcLocation, new KeyValuePair<string, int>(PartName, 2));
    
    
    [Fact]
    public async Task ShouldHaveCorrectNumberOfPlacesAndTransitions()
    {
        PetriNetComponent component = await _translater.TranslateAsync(Move);

        using (new AssertionScope())
        {
            component.Transitions.Should().HaveCount(1, "CStart2P should be translated to a single transition");
            component.Places.Should().HaveCount(4, "the component translates into 4 places");
        }
    }
    
    
    [Fact]
    public async Task ShouldHaveCorrectInvariants()
    {
        PetriNetComponent component = await _translater.TranslateAsync(Move);
        var places = component.Places;
        var cproc = places.First(e => e.Name == ProcLocation.Name);
        var cprocCap = places.First(e => e.Name == CapacityPlaceCreator.CapacityNameFor(ProcLocation.Name));
        var cbufCap = places.First(e => e.Name == BufferLocation.Name);
        var cbuf = places.First(e => e.Name == CapacityPlaceCreator.CapacityNameFor(BufferLocation.Name));
        
        
        using (new AssertionScope())
        {
            cproc.ColorInvariants.Should().Contain(e => e.Key == PartName && e.Value == ProcMaxAge);
            cbuf.ColorInvariants.Should().Contain(e => e.Key == PartName && e.Value == InfinityInteger.Positive);
            
            cprocCap.ColorInvariants.Should().Contain(e => e.Key == CapacityColor && e.Value == InfinityInteger.Positive, "Capacity places should have infinity invariant and should have color dot.");
            cbufCap.ColorInvariants.Should().Contain(e => e.Key == CapacityColor && e.Value == InfinityInteger.Positive, "Capacity places should have infinity invariant and should have color dot.");
        }
    }
    
    
    [Fact]
    public async Task ShouldHaveCorrectConsumptionsForIngoing()
    {
        PetriNetComponent component = await _translater.TranslateAsync(Move);
        ICollection<IngoingArc> arcs = component.Transitions.First().InGoing;
        var bufferHat = arcs.First(e => e.From.Name == BufferLocation.CapacityName());
        var buffer = arcs.First(e => e.From.Name == BufferLocation.Name);
        var procHat = arcs.First(e => e.From.Name == ProcLocation.CapacityName());
        
        using (new AssertionScope())
        {
            //cproc-hat
            procHat.Guards.Should().HaveCount(1, "single part type example should only have single guard");
            procHat.Amounts.Should().HaveCount(1, "only one part type is moved");
            procHat.Amounts.First().Key.Should().Be(CapacityColor);
            procHat.Amounts.First().Value.Should().Be(2);
            
            ColoredGuard procHatGuard = procHat.Guards.First();
            procHatGuard.ColorIntervals.Should().HaveCount(1, "Only a single color guards should be defined for this action");
            procHatGuard.ShouldHaveFirstColorIntervalBeDotWithZeroToInfinity();
            
            //cbuffer-hat
            bufferHat.Amounts.Should().HaveCount(1, "only one part type is moved, which is dot");
            bufferHat.Guards.Should().HaveCount(1, "buffer cap should only have dot guard with infinity");
            bufferHat.Amounts.First().Key.Should().Be(CapacityColor);
            bufferHat.Amounts.First().Value.Should().Be(2);
            
            ColoredGuard bufferCapGuard = bufferHat.Guards.First();
            bufferCapGuard.ColorIntervals.Should().HaveCount(1, "Only a single color guards should be defined for this action");
            bufferCapGuard.ShouldHaveFirstColorIntervalBeDotWithZeroToInfinity();

            //cbuffer
            buffer.Amounts.Should().HaveCount(1);
            buffer.Guards.Should().HaveCount(1);
            buffer.Amounts.First().Key.Should().Be(PartName);
            buffer.Amounts.First().Value.Should().Be(2);
            
            ColoredGuard procBufferGuard = buffer.Guards.First();
            procBufferGuard.ColorIntervals.Should().HaveCount(1, "Only a single color guards should be defined for this action");
            procBufferGuard.ShouldBeOfColorAndHaveZeroToInfinity(PartName);
        }
    }
    
    [Fact]
    public async Task ShouldHaveCorrectProductionsForOutgoings()
    {
        PetriNetComponent component = await _translater.TranslateAsync(Move);
        var arcs = component.Transitions.First().OutGoing;
        var cProcArc = arcs.First(e => e.To.Name == ProcLocation.Name);
        var cBufferArc = arcs.First(e => e.To.Name == ProcLocation.Name);
        
        
        using (new AssertionScope())
        {
            arcs.Should().HaveCount(2, "we put parts into both cbuffer-hat and cproc");
            
            cProcArc.Amounts.Should().HaveCount(1);
            cProcArc.Amounts.First().Key.Should().Be(PartName, "it is not a capacity location");
            cProcArc.Amounts.First().Value.Should().Be(2, "two parts are moved");

            cBufferArc.Amounts.Should().HaveCount(1);
            cProcArc.Amounts.First().Key.Should().Be(CapacityColor);
            cProcArc.Amounts.First().Value.Should().Be(2, "two parts are moved out of the location");
        }
    }
    

    [Fact]
    public async Task ShouldHaveCorrectNumberOfArcs()
    {
        PetriNetComponent component = await _translater.TranslateAsync(Move);
        Transition transition = component.Transitions.First();

        using (new AssertionScope())
        {
            component.Transitions.Should().HaveCount(1, "CStart2P should be translated to a single transition");
            transition.InGoing.Should().HaveCount(3);
            transition.InhibitorArcs.Should().HaveCount(1);
            transition.OutGoing.Should().HaveCount(2);
            transition.ShouldHaveArcsFrom(BufferLocation.Name, CapacityPlaceCreator.CapacityNameFor(BufferLocation.Name), CapacityPlaceCreator.CapacityNameFor(ProcLocation.Name));
            transition.ShouldHaveArcsTo(ProcLocation.Name, BufferLocation.Name);
        }
    }
    
    [Fact]
    public async Task ShouldHaveCorrectInhibitorArcGuard()
    {
        PetriNetComponent component = await _translater.TranslateAsync(Move);
        Transition transition = component.Transitions.First();
        CountCollection<string> inhibitorConsumption = transition.InhibitorArcs.Select(e => e.Amounts).First();

        using (new AssertionScope())
        {
            transition.InhibitorArcs.Should().HaveCount(1);
            inhibitorConsumption.Should().HaveCount(1);
            inhibitorConsumption.First().Key.Should().Be(PartName);
            inhibitorConsumption.First().Key.Should().Be(PartName);
            inhibitorConsumption.First().Value.Should().Be(1);
        }
    }
    
    [Fact]
    public async Task ShouldHaveInhibitorArcGuardWithSingleWeight()
    {
        PetriNetComponent component = await _translater.TranslateAsync(Move);
        Transition transition = component.Transitions.First();
        CountCollection<string> inhibitorConsumption = transition.InhibitorArcs.Select(e => e.Amounts).First();

        using (new AssertionScope())
        {
            transition.InhibitorArcs.Should().HaveCount(1);
            inhibitorConsumption.Should().HaveCount(1);
            inhibitorConsumption.First().Key.Should().Be(PartName);
            inhibitorConsumption.First().Key.Should().Be(PartName);
            inhibitorConsumption.First().Value.Should().Be(1);
        }
    }
}