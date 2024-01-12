using TACPN;
using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Net;
using TACPN.Places;
using TACPN.Transitions;
using Tmpms.Common;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class FromLocationAdaption : ITransitionAttachable
{
    private readonly JourneyCollection collection;

    /// <summary>
    /// </summary>
    /// <param name="fromLocation"> A place representing a location l.</param>
    /// <param name="partsToConsume">The parts that need to be consumed from l.</param>
    public FromLocationAdaption(Location fromLocation, IEnumerable<KeyValuePair<string, int>> partsToConsume, JourneyCollection collection)
    {
        FromLocation = fromLocation;
        ToConsume = partsToConsume;
        (FromPlace, FromPlaceHat) = LocationTranslator.CreatePlaceAndCapacityPlacePair(fromLocation, collection);
        this.collection = collection;
    }

    private Location FromLocation { get; set; }

    private IEnumerable<KeyValuePair<string, int>> ToConsume { get; set; }

    private CapacityPlace FromPlaceHat { get; set; }

    private Place FromPlace { get; set; }

    public void AttachToTransition(Transition transition)
    {
        AdaptPlace(transition);
        AdaptCapacityPlace(transition);
    }   

    private void AdaptCapacityPlace(Transition transition)
    {
        var consProdAmount = ToConsume.Sum(e => e.Value);
        var expression = new ColourExpression(ColourType.DefaultColorType.ColourValue, ColourType.DefaultColorType, consProdAmount);
        transition.AddOutGoingTo(FromPlaceHat, expression );
    }

    private void AdaptPlace(Transition transition)
    {
        var guards = ToConsume.Select(pair =>
        {
            var first = FromLocation.Invariants.First(e => e.PartType == pair.Key);
            return ColourTimeGuard.TokensGuard(first.Min, first.Max);
        });
        
        var expression = ColourExpressions.MovePartsExpression(ToConsume);
        transition.AddInGoingFrom(FromPlace,guards,expression);
    }
}