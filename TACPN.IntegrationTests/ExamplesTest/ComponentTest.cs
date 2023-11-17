using Common.Factories;
using Common.JsonTofms;
using Common.JsonTofms.ConsistencyCheck.Validators;
using Common.Move;
using Common.Translate;
using TACPN.Adapters;
using TACPN.Adapters.Location;
using TACPN.Net;
using TACPN.Net.Transitions;

namespace TACPN.IntegrationTests.ExamplesTest;

public abstract class ComponentTest {
    protected readonly IMoveActionTranslation<PetriNetComponent> Translater = new TofmToTacpnTranslater(new MoveActionAdapterFactory());
    
    protected readonly JsonTofmToDomainTofmParser JsonParser;
    protected readonly string JsonText;
    protected static readonly string Hat = CapacityPlaceCreator.Hat;
    protected static readonly string dot = CapacityPlaceCreator.DefaultColorName;
    public ComponentTest(string text)
    {
        TofmSystemValidator systemValidator = new TofmSystemValidator(new LocationValidator(new InvariantValidator()),new NamingValidator(), new MoveActionValidator());
        JsonText = text;
        new TofmSystemValidator(
            new LocationValidator(new InvariantValidator()),
            new NamingValidator(),
            new MoveActionValidator()
        );

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