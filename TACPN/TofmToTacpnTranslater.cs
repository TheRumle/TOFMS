using Common;
using Common.Move;
using Common.Translate;
using TACPN.Net;
using TACPN.Net.Transitions;

namespace TACPN;

public static class CapacityPlaceCreator
{

    public static readonly string DefaultColorName = "dot";
    public static string CapacityNameFor(Place place)
    {
        return place.Name + "_buffer";
    }
    
    public static string CapacityNameFor(string name)
    {
        return name + "_buffer";
    }
    
    public static Place CapacityPlaceFor(Place place)
    {
        var infinityPlaces = place.ColorInvariants.Select(kvp => 
            new KeyValuePair<string, int>(DefaultColorName, InfinityInteger.Positive));
        return new Place(CapacityNameFor(place), infinityPlaces);
    }
}

public class TofmToTacpnTranslater : IMoveActionTranslation<PetriNetComponent>
{
    private async Task<Transition> ExtractTransition(string name)
    {
        return await Task.Run(() =>
        {
            return new Transition(name);
        });
    }
    
    public PetriNetComponent Translate(MoveAction moveAction)
    {
        var name = moveAction.Name;


            return null;
    }

    public Task<PetriNetComponent> TranslateAsync(MoveAction moveAction)
    {
        return Task.Run(PetriNetComponent.Empty);
    }
}