using TACPN.Adapters.TofmToTacpnAdapter;
using TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;
using Tofms.Common;
using Tofms.Common.Move;

namespace ConsoleApp.ProgramBuilder;

public class TranslateToTacpn
{
    private readonly IEnumerable<MoveAction> moveActions;
    private readonly Dictionary<string, List<Location>> _journeys;
    public readonly ParseAndValidateTofmSystem PrevStep;

    public TranslateToTacpn(IEnumerable<MoveAction> moveActions, Dictionary<string,  List<Location>> journeys,  ParseAndValidateTofmSystem parseAndValidateTofmSystem)
    {
        this.moveActions = moveActions;
        _journeys = journeys;
        this.PrevStep = parseAndValidateTofmSystem;
    }

    public ExtractTacpnXmlFormat TranslateTofmsToTacpnComponents()
    {

        IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> j = _journeys.Select(e =>
        {
            var k = e.Key!;
            var values = e.Value;
            var newValues = values.Select(h => KeyValuePair.Create(values.IndexOf(h), h));
            return KeyValuePair.Create(k, newValues);
        });

        var journey = new JourneyCollection(j);


        var translater = new TofmToTacpnTranslater(new MoveActionToTransitionFactory(journey), journey);
        var components = moveActions
            .Select( e=> translater.TranslateAsync(e))
            .Select(e=>
            {
                e.Wait();
                return e.Result;
            });

        return new ExtractTacpnXmlFormat(components, this, journey);
    }
}