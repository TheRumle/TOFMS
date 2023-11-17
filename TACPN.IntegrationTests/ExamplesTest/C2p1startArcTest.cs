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

public class C2p1startArcTest : IClassFixture<CentrifugeFixture>
{
    private readonly TofmToTacpnTranslater _translater = new(new MoveActionAdapterFactory());
    
    private readonly JsonTofmToDomainTofmParser _jsonParser;
    private readonly string _centrifugeText;
    private static string hat = CapacityPlaceCreator.Hat;
    private static string dot = CapacityPlaceCreator.DefaultColorName;

    public C2p1startArcTest(CentrifugeFixture fixture)
    {
        TofmSystemValidator systemValidator = new TofmSystemValidator(new LocationValidator(new InvariantValidator()),new NamingValidator(), new MoveActionValidator());
        this._centrifugeText = fixture.ComponentText;
        new TofmSystemValidator(
            new LocationValidator(new InvariantValidator()),
            new NamingValidator(),
            new MoveActionValidator()
        );

        this._jsonParser = new JsonTofmToDomainTofmParser(systemValidator, new MoveActionFactory()); 
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
            var cbufferIngoing = transition.FindFirstIngoingFromPlaceContaining("cbuffer");
            cbufferIngoing.Guards.Should().HaveCount(1);
            cbufferIngoing.Guards.First().ShouldHaveLabels("p1", 2);
        }
    }
    
    [Fact]
    public async Task ShouldHave_InhibitorArc_With_Weight1_From_CProc()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var cbufferIngoing = transition.FindFirstIngoingFromPlaceContaining("cproc");
            cbufferIngoing.Guards.Should().HaveCount(1);
            cbufferIngoing.Guards.First().ShouldHaveLabels(dot, 2);
        }
    }
    
    [Fact]
    public async Task ShouldHave_IngoingArc_With_dotInfiniteAge_From_CProcHat()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            var cbufferIngoing = transition.FindFirstIngoingFromPlaceContaining("cproc", hat);
            cbufferIngoing.Guards.Should().HaveCount(1);
            cbufferIngoing.Guards.First().ShouldHaveLabels(dot, 2);
        }
    }
    
    [Fact]
    public async Task ShouldHave_IngoingArc_With_dotInfiniteAge_From_CBufferHat()
    {
        Transition transition = await GetFirstTransition();

        using (new AssertionScope())
        {
            IngoingArc cbufferIngoing = transition.FindFirstIngoingFromPlaceContaining("cbuffer", hat);
            cbufferIngoing.Guards.Should().HaveCount(1);
            cbufferIngoing.Guards.First().ShouldHaveLabels(dot, 2);
        }
    }
    
    private async Task<MoveAction> GetAction()
    {
        var a = await _jsonParser.ParseTofmComponentJsonString(this._centrifugeText);
        var action = a.FirstOrDefault(e => e.Name.ToLower().Contains("start") && e.Name.ToLower().Contains("2p1"));
        if (action is null) throw new ArgumentException("No action with name containing both 'start' and '2p1'");
        return action;
    }

    public async Task<Transition> GetFirstTransition()
    {
        var component = await _translater.TranslateAsync(await GetAction());
        return component.Transitions.First();
    }
    
}