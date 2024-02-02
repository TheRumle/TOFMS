using TACPN;
using TACPN.Colours.Type;
using TACPN.Places;
using TACPN.Transitions;
using TACPN.Transitions.Guard;
using Tmpms.Common;
using Tmpms.Common.Move;
using Tmpms.Common.Translate;

namespace TmpmsPetriNetAdapter;

public class TofmToTacpnTranslater(ITransitionAttachableFactory transitionAttachableFactory, ITransitionFactory transitionFactory,
        IndexedJourney indexedJourneys)
    : IMoveActionTranslation<PetriNetComponent>
{

    public PetriNetComponent Translate(MoveAction moveAction)
    {
        Transition transition = transitionFactory.CreateTransition(moveAction);
        transitionAttachableFactory.AdaptFrom(moveAction).AttachToTransition(transition);
        transitionAttachableFactory.AdaptEmptyAfter(moveAction).AttachToTransition(transition);
        transitionAttachableFactory.AdaptTo(moveAction).AttachToTransition(transition);
        transitionAttachableFactory.AdaptEmptyBefore(moveAction).AttachToTransition(transition);

        var processingLocationsForAction = moveAction.InvolvedLocations.Where(e => e.IsProcessing).Select(e=>e.Name);
        IEnumerable<string> involvedParts = moveAction.PartsToMove.Select(e => e.Key);
        
        
        foreach (var kv in indexedJourneys)
        {
            var part = kv.Key;
            var locations = kv.Value;
            if (!involvedParts.Contains(part)) continue;
            
        }

        

        var colourTypes = transition.InvolvedPlaces.Select(e => e.ColourType).DistinctBy(e=>e.Name);
        return new PetriNetComponent
        {
            InvolvedParts = moveAction.PartsToMove.Select(e => e.Key),
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