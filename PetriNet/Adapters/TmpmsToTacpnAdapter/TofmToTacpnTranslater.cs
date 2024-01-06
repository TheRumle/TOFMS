using TACPN.Net;
using TACPN.Net.Colours.Type;
using TACPN.Net.Places;
using TACPN.Net.Transitions;
using TACPN.Net.Transitions.Guard;
using Tmpms.Common;
using Tmpms.Common.Move;
using Tmpms.Common.Translate;

namespace TACPN.Adapters.TmpmsToTacpnAdapter;

public class TofmToTacpnTranslater : IMoveActionTranslation<PetriNetComponent>
{
    private readonly ITransitionAttachableFactory _transitionAttachableFactory;
    private readonly Dictionary<string, IEnumerable<KeyValuePair<int, Location>>> _journeysCollection;

    public TofmToTacpnTranslater(ITransitionAttachableFactory transitionAttachableFactory, JourneyCollection journeysCollection)
    {
        _transitionAttachableFactory = transitionAttachableFactory;
        _journeysCollection = journeysCollection;
    }

    public PetriNetComponent Translate(MoveAction moveAction)
    {
        var transition = new Transition(moveAction.Name, ColourType.TokenAndDefaultColourType, TransitionGuard.Empty( ColourType.TokensColourType) );

        _transitionAttachableFactory.AdaptFrom(moveAction).AttachToTransition(transition);
        _transitionAttachableFactory.AdaptEmptyAfter(moveAction).AttachToTransition(transition);
        _transitionAttachableFactory.AdaptTo(moveAction).AttachToTransition(transition);
        _transitionAttachableFactory.AdaptEmptyBefore(moveAction).AttachToTransition(transition);

        var processingLocationsForAction = moveAction.InvolvedLocations.Where(e => e.IsProcessing).Select(e=>e.Name);
        IEnumerable<string> involvedParts = moveAction.PartsToMove.Select(e => e.Key);
        
        
        foreach (var kv in _journeysCollection)
        {
            var part = kv.Key;
            var locations = kv.Value;
            if (!involvedParts.Contains(part)) continue;
            
        }

        

        var colourTypes = transition.InvolvedPlaces.Select(e => e.ColourType).DistinctBy(e=>e.Name);
        return new PetriNetComponent
        {
            Colors = moveAction.PartsToMove.Select(e => e.Key),
            Transitions = new List<Transition> { transition },
            Places = transition.InvolvedPlaces.OfType<Place>().ToList(),
            CapacityPlaces = transition.InvolvedPlaces.OfType<CapacityPlace>().ToList(),
            ColourTypes = colourTypes,
            Name = moveAction.Name + "Component",
        };
    }

    public async Task<PetriNetComponent> TranslateAsync(MoveAction moveAction)
    {
        return await Task.Run(() => Translate(moveAction));
    }
}