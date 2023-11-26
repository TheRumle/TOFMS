﻿using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;

namespace TACPN.Adapters.TofmToTacpnAdapter.LocationAdapters;

internal class FromLocationAdaption : ITransitionAttachable
{
    /// <summary>
    /// </summary>
    /// <param name="fromLocation"> A place representing a location l.</param>
    /// <param name="partsToConsume">The parts that need to be consumed from l.</param>
    public FromLocationAdaption(Tofms.Common.Location fromLocation, IEnumerable<KeyValuePair<string, int>> partsToConsume)
    {
        FromLocation = fromLocation;
        ToConsume = partsToConsume;
        (FromPlace, FromPlaceHat) = LocationTranslator.CreateLocationPlacePair(fromLocation);
    }

    private Tofms.Common.Location FromLocation { get; set; }

    private IEnumerable<KeyValuePair<string, int>> ToConsume { get; set; }

    private Place FromPlaceHat { get; set; }

    private Place FromPlace { get; set; }

    public void AttachToTransition(Transition transition)
    {
        AdaptPlace(transition);
        AdaptCapacityPlace(transition);
    }

    private void AdaptCapacityPlace(Transition transition)
    {
        var consProdAmount = ToConsume.Sum(e => e.Value);
        var capPlaceColor = CapacityPlaceCreator.DefaultColorName;
        transition.AddOutGoingTo(FromPlaceHat, new Production(capPlaceColor, consProdAmount));
        transition.AddInGoingFrom(FromPlaceHat,
            new ColoredGuard(consProdAmount, capPlaceColor, Interval.ZeroToInfinity));
    }

    private void AdaptPlace(Transition transition)
    {
        var guards = ToConsume.Select(pair =>
        {
            var first = FromLocation.Invariants.First(e => e.PartType == pair.Key);
            return new ColoredGuard(pair.Value, pair.Key, new Interval(first.Min, first.Max));
        });

        transition.AddInGoingFrom(FromPlace, guards);
    }
}