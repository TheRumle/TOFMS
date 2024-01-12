using TACPN.Adapters.TmpmsToTacpnAdapter;
using TACPN.Adapters.TmpmsToTacpnAdapter.TransitionAttachable;
using TACPN.Net;
using TACPN.Places;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Factories;
using Tmpms.Common.JsonTofms;
using Tmpms.Common.JsonTofms.ConsistencyCheck.Validators;
using Tmpms.Common.Move;

namespace TACPN.IntegrationTests.ExamplesTest;

public abstract class ComponentTest {
    
    
    private static JourneyCollection collection = new JourneyCollection(new List<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>>());

    protected readonly JsonTofmToDomainTofmParser JsonParser;
    protected readonly string JsonText;
    protected static readonly string Hat = CapacityPlaceExtensions.Hat;
    protected static readonly string dot = CapacityPlaceExtensions.DefaultCapacityColor;
    public TofmToTacpnTranslater Translater = new TofmToTacpnTranslater(new MoveActionToTransitionFactory(collection), collection);
    public ComponentTest(string text)
    {
        TofmSystemValidator systemValidator = new TofmSystemValidator(new LocationValidator(new InvariantValidator()),new NamingValidator(), new MoveActionValidator());
        JsonText = text;
        JsonParser = new JsonTofmToDomainTofmParser(systemValidator, new MoveActionFactory()); 
        
    }
    
        
    protected async Task<MoveAction> GetAction()
    {
        IEnumerable<MoveAction> actions = await JsonParser.ParseTofmComponentJsonString(JsonText);
        var action = actions.FirstOrDefault(e => e.Name.ToLower().Contains("start") && e.Name.ToLower().Contains("2p1"));
        if (action is null) throw new ArgumentException("No action with name containing both 'start' and '2p1'");
        return action;
    }

    protected async Task<Transition> GetFirstTransition()
    {
        var component = await Translater.TranslateAsync(await GetAction());
        
        return component.Transitions.First();
    }
}