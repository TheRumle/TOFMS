using TACPN;
using TACPN.Colours.Type;
using TACPN.Net;
using TACPN.Places;
using TACPN.Transitions;
using TACPN.Transitions.Guard;
using Tmpms.Common;
using Tmpms.Common.Move;
using Tmpms.Common.Translate;

namespace TmpmsPetriNetAdapter;

public class TofmToTacpnTranslater(ITransitionAttachableFactory transitionAttachableFactory,
        IndexedJourney indexedJourneys)
    : IMoveActionTranslation<PetriNetComponent>
{
    private readonly Dictionary<string, IEnumerable<KeyValuePair<int, Location>>> _indexedJourneys = indexedJourneys;

    public PetriNetComponent Translate(MoveAction moveAction)
    {
        //TODO transition factory
        Transition transition = new Transition(moveAction.Name, ColourType.TokenAndDefaultColourType, TransitionGuard.Empty());
        transitionAttachableFactory.AdaptFrom(moveAction).AttachToTransition(transition);
        transitionAttachableFactory.AdaptEmptyAfter(moveAction).AttachToTransition(transition);
        transitionAttachableFactory.AdaptTo(moveAction).AttachToTransition(transition);
        transitionAttachableFactory.AdaptEmptyBefore(moveAction).AttachToTransition(transition);

        var processingLocationsForAction = moveAction.InvolvedLocations.Where(e => e.IsProcessing).Select(e=>e.Name);
        IEnumerable<string> involvedParts = moveAction.PartsToMove.Select(e => e.Key);
        
        
        foreach (var kv in _indexedJourneys)
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