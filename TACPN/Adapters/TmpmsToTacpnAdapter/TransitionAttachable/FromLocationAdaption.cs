using TACPN.Net;
using TACPN.Net.Colours.Evaluatable;
using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;
using TACPN.Net.Places;
using TACPN.Net.Transitions;
using Tmpms.Common;

namespace TACPN.Adapters.TmpmsToTacpnAdapter.TransitionAttachable;

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
        var expression = new ColourExpression(Colour.DefaultTokenColour, ColourType.DefaultColorType, consProdAmount);
        transition.AddOutGoingTo(FromPlaceHat, expression );
    }

    private void AdaptPlace(Transition transition)
    {
        var guards = ToConsume.Select(pair =>
        {
            var first = FromLocation.Invariants.First(e => e.PartType == pair.Key);
            return ColoredGuard.TokensGuard(first.Min, first.Max);
        });
        
        var expression = ColourExpressions.MovePartsExpression(ToConsume);
        transition.AddInGoingFrom(FromPlace,guards, expression);
    }
}