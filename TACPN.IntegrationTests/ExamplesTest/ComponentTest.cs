using TACPN.Adapters.TofmToTacpnAdapter;
using TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;
using TACPN.Net;
using TACPN.Net.Transitions;
using Tofms.Common;
using Tofms.Common.Factories;
using Tofms.Common.JsonTofms;
using Tofms.Common.JsonTofms.ConsistencyCheck.Validators;
using Tofms.Common.Move;
using Tofms.Common.Translate;

namespace TACPN.IntegrationTests.ExamplesTest;

public abstract class ComponentTest {
    
    protected readonly IMoveActionTranslation<PetriNetComponent> Translater = new TofmToTacpnTranslater(new MoveActionToTransitionFactory(), new Dictionary<string, IEnumerable<Location>>());
    
    protected readonly JsonTofmToDomainTofmParser JsonParser;
    protected readonly string JsonText;
    protected static readonly string Hat = CapacityPlaceExtensions.Hat;
    protected static readonly string dot = CapacityPlaceExtensions.DefaultCapacityColor;
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