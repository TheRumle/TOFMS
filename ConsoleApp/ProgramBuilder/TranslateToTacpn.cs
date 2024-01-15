using Tmpms.Common;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter;
using TmpmsPetriNetAdapter.TransitionAttachable;

namespace ConsoleApp.ProgramBuilder;

public class TranslateToTacpn
{
    public readonly TimedMultipartSystem TimedMultipartSystem;
    private readonly IEnumerable<MoveAction> _moveActions;
    private readonly IReadOnlyDictionary<string, IEnumerable<Location>> _journeys;
    public readonly ParseAndValidateTofmSystem PrevStep;

    public TranslateToTacpn(TimedMultipartSystem timedMultipartSystem, IEnumerable<MoveAction> moveActions,
        IReadOnlyDictionary<string, IEnumerable<Location>> journeys, ParseAndValidateTofmSystem parseAndValidateTofmSystem)
    {
        TimedMultipartSystem = timedMultipartSystem;
        this._moveActions = moveActions;
        _journeys = journeys;
        this.PrevStep = parseAndValidateTofmSystem;
    }

    public ExtractTacpnXmlFormat TranslateTofmsToTacpnComponents()
    {

        var j = _journeys.Select(e =>
        {
            var k = e.Key!;
            var values = e.Value;
            var newValues = values.Select((h,index) => KeyValuePair.Create(index, h));
            return KeyValuePair.Create(k, newValues);
        });

        var journey = new JourneyCollection(j);


        var translater = new TofmToTacpnTranslater(new MoveActionToTransitionFactory(journey), journey);
        var components = _moveActions
            .Select( e=> translater.TranslateAsync(e))
            .Select(e=>
            {
                e.Wait();
                return e.Result;
            });
        
        var transition = components.First().Transitions.First();
        return new ExtractTacpnXmlFormat(components, this, journey);
    }
}