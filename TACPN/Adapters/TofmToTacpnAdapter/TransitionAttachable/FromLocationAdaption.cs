﻿using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;

namespace TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;

internal class FromLocationAdaption : ITransitionAttachable
{
    private readonly JourneyCollection collection;

    /// <summary>
    /// </summary>
    /// <param name="fromLocation"> A place representing a location l.</param>
    /// <param name="partsToConsume">The parts that need to be consumed from l.</param>
    public FromLocationAdaption(Tofms.Common.Location fromLocation, IEnumerable<KeyValuePair<string, int>> partsToConsume, JourneyCollection collection)
    {
        FromLocation = fromLocation;
        ToConsume = partsToConsume;
        (FromPlace, FromPlaceHat) = LocationTranslator.CreatePlaceAndCapacityPlacePair(fromLocation, collection);
        this.collection = collection;
    }

    private Tofms.Common.Location FromLocation { get; set; }

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
        var capPlaceColor = CapacityPlaceExtensions.DefaultCapacityColor;
        transition.AddOutGoingTo(FromPlaceHat, new Production(ColourType.DefaultColorType, consProdAmount));
    }

    private void AdaptPlace(Transition transition)
    {
        var amount = ToConsume.Sum(e => e.Value);
        var guards = ToConsume.Select(pair =>
        {
            var first = FromLocation.Invariants.First(e => e.PartType == pair.Key);
            return ColoredGuard.TokensGuard(amount, first.Min, first.Max);
        });
        transition.AddInGoingFrom(FromPlace,guards);
    }
}