using Common.Factories;
using Common.JsonTofms;
using Common.JsonTofms.ConsistencyCheck.Validators;
using Common.Move;
using FluentAssertions;
using FluentAssertions.Execution;
using JsonFixtures.Fixtures;
using TACPN.Adapters;
using TACPN.Adapters.Location;
using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;
using Xunit;

namespace TACPN.IntegrationTests.ExamplesTest;

public class C2P1StartArcTest : IClassFixture<CentrifugeFixture>
{
    private readonly TofmToTacpnTranslater _translater = new(new MoveActionAdapterFactory());
    
    private readonly JsonTofmToDomainTofmParser _jsonParser;
    private readonly string _centrifugeText;
    private static string hat = CapacityPlaceCreator.Hat;
    private static string dot = CapacityPlaceCreator.DefaultColorName;

    public C2P1StartArcTest(CentrifugeFixture fixture)
    {
        TofmSystemValidator systemValidator = new TofmSystemValidator(new LocationValidator(new InvariantValidator()),new NamingValidator(), new MoveActionValidator());
        _centrifugeText = fixture.ComponentText;
        new TofmSystemValidator(
            new LocationValidator(new InvariantValidator()),
            new NamingValidator(),
            new MoveActionValidator()
        );

        _jsonParser = new JsonTofmToDomainTofmParser(systemValidator, new MoveActionFactory()); 
    }
    

    [Fact]
    public async Task ShouldHave_OneTransition_With_ThreeIngoing_TwoOutgoing_OneInhibitor()
    {
        MoveAction action = await GetAction();
        var net = await _translater.TranslateAsync(action);

        net.Transitions.Should().HaveCount(1);
        var transition = net.Transitions.First();
        transition.InGoing.Should().HaveCount(3);
        transition.OutGoing.Should().HaveCount(2);
        transition.InhibitorArcs.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task ShouldHave_IngoingArc_With_2p1InfiniteAge_From_CBuffer()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.FindFirstIngoingFromPlaceContaining("cbuffer");
            arc.Guards.Should().HaveCount(1);
            arc.Guards.First().ShouldHaveLabels("p1", 2);
        }
    }
    
    [Fact]
    public async Task ShouldHave_InhibitorArc_With_Weight1_From_CProc()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.FindFirstIngoingFromPlaceContaining("cproc");
            arc.Guards.Should().HaveCount(1);
            arc.Guards.First().ShouldHaveLabels(dot, 2);
        }
    }
    
    [Fact]
    public async Task ShouldHave_IngoingArc_With_dotInfiniteAge_From_CProcHat()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.FindFirstIngoingFromPlaceContaining("cproc", hat);
            arc.Guards.Should().HaveCount(1);
            arc.Guards.First().ShouldHaveLabels(dot, 2);
        }
    }
    
    [Fact]
    public async Task ShouldHave_IngoingArc_With_dotInfiniteAge_From_CBufferHat()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            IngoingArc arc = transition.FindFirstIngoingFromPlaceContaining("cbuffer", hat);
            arc.Guards.Should().HaveCount(1);
            arc.Guards.First().ShouldHaveLabels(dot, 2);
        }
    }
    
    [Fact]
    public async Task ShouldHave_OutGoing_With_2P1_To_CProc()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.FindFirstOutgoingToPlaceContaining("cproc");
            arc.Productions.Should().HaveCount(1);
            Production production = arc.Productions.First();
            production.Color.ToLower().Should().Be("p1");
            production.Amount.Should().Be(2);
        }
    }
    
    [Fact]
    public async Task ShouldHave_OutGoing_With_2Dot_To_BufferHat()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var arc = transition.FindFirstOutgoingToPlaceContaining("cbuffer", hat);
            arc.Productions.Should().HaveCount(1);
            Production production = arc.Productions.First();
            production.Color.Should().Be(dot);
            production.Amount.Should().Be(2);
        }
    }
    
    private async Task<MoveAction> GetAction()
    
    {
        IEnumerable<MoveAction> actions = await _jsonParser.ParseTofmComponentJsonString(this._centrifugeText);
        var action = actions.FirstOrDefault(e => e.Name.ToLower().Contains("start") && e.Name.ToLower().Contains("2p1"));
        if (action is null) throw new ArgumentException("No action with name containing both 'start' and '2p1'");
        return action;
    }

    public async Task<Transition> GetFirstTransition()
    {
        var component = await _translater.TranslateAsync(await GetAction());
        return component.Transitions.First();
    }
    
}