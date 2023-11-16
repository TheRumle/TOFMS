using System.Reflection.Metadata.Ecma335;
using Common;
using Common.JsonTofms.Models;
using Common.Move;
using Common.Translate;
using TACPN.Net;
using TACPN.Net.Transitions;

namespace TACPN;

public class TofmToTacpnTranslater : IMoveActionTranslation<PetriNetComponent>
{
    private async Task<Transition> ExtractTransition(string name)
    {
        return await Task.Run(() =>
        {
            return new Transition(name);
        });
    }

    public (Place to, Place toHat) CreateLocationPlacePair(Location location)
    {
        Place to = new Place(location.Name,
            location.Invariants.Select(e => new KeyValuePair<string, int>(e.PartType, e.Max)));
        
        Place toHat = CapacityPlaceCreator.CapacityPlaceFor(to);
        return (to, toHat);
    }
    
    
    
    public (Place from, Place fromHat) TranslateFrom(Location location)
    {
        Place from = new Place(location.Name,
            location.Invariants.Select(e => new KeyValuePair<string, int>(e.PartType, e.Max)));

        Place toHat = CapacityPlaceCreator.CapacityPlaceFor(from);
        return (from, toHat);
    }
    
    public PetriNetComponent Translate(MoveAction moveAction)
    {
        Transition transition = new Transition(moveAction.Name);
        var fromAdapter = new FromLocationAdaption(moveAction.From, moveAction.PartsToMove);
        fromAdapter.AdaptToTransition(transition);






        return new PetriNetComponent()
        {
            Colors = moveAction.PartsToMove.Select(e => e.Key),
            Transitions = new List<Transition>() { transition },
            Places = new List<Place>()
        };
    }

    public Task<PetriNetComponent> TranslateAsync(MoveAction moveAction)
    {
        return Task.Run(()=>Translate(moveAction));
    }
}