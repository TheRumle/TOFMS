using TACPN;
using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Places;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class FromLocationAdaption : Adapter
{

    /// <summary>
    /// </summary>
    /// <param name="fromLocation"> A place representing a location l.</param>
    /// <param name="partsToConsume">The parts that need to be consumed from l.</param>
    public FromLocationAdaption(Location fromLocation, IEnumerable<KeyValuePair<string, int>> partsToConsume,
        IndexedJourneyCollection collection, ColourType ct):base(ct, collection)
    {
        FromLocation = fromLocation;
        ToConsume = partsToConsume;
        (FromPlace, FromPlaceHat) = LocationTranslator.CreatePlaceAndCapacityPlacePair(fromLocation, collection,ct);
    }

    private Location FromLocation { get; set; }

    private IEnumerable<KeyValuePair<string, int>> ToConsume { get; set; }

    private CapacityPlace FromPlaceHat { get; set; }

    private Place FromPlace { get; set; }

    public override void AttachToTransition(Transition transition)
    {
        AdaptPlace(transition);
        AdaptCapacityPlace(transition);
    }   

    private void AdaptCapacityPlace(Transition transition)
    {
        var consProdAmount = ToConsume.Sum(e => e.Value);
        var expression = new ColourExpression(Colour.DefaultTokenColour, ColourType.DefaultColorType, consProdAmount);
        transition.AddOutGoingTo(FromPlaceHat, expression );
    }

    private void AdaptPlace(Transition transition)
    {
        var guards = ToConsume.Select(pair =>
        {
            var first = FromLocation.Invariants.First(e => e.PartType == pair.Key);
            return ColourTimeGuard.TokensGuard(first.Min, first.Max,this.PartColourType);
        });
        
        var a = journeyColourFactory.CreatePartMoveTuple(ToConsume, FromPlace, _collection);
        transition.AddInGoingFrom(FromPlace, guards,a);
    }
}