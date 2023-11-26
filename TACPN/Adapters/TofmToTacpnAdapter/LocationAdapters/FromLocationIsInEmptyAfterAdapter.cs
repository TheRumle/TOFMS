using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;
using Tofms.Common;

namespace TACPN.Adapters.TofmToTacpnAdapter.LocationAdapters;

internal class FromLocationIsInEmptyAfterAdapter : ITransitionAttachable
{
    private readonly ISet<Location> _emptyAfter;
    private readonly Location _fromLocation;
    private readonly int _guardAmount;

    public FromLocationIsInEmptyAfterAdapter(ISet<Location> emptyAfterLocations, Location fromLocation,
        IEnumerable<KeyValuePair<string, int>> partsToConsume)
    {
        _emptyAfter = emptyAfterLocations;
        _fromLocation = fromLocation;
        _guardAmount =  fromLocation.Capacity - partsToConsume.Sum(e=>e.Value) - 1;
    }

    public void AttachToTransition(Transition transition)
    {
        if (TryGetExistingIngoingFromFromHat(transition, out var ingoingFromFromHat))
        {
            ModifyExistingArc(ingoingFromFromHat!);
            return;
        }
        
        var (_, fromPlaceHat) = LocationTranslator.CreateLocationPlacePair(_fromLocation);
        AppendArcs(transition, fromPlaceHat);
    }

    private void ModifyExistingArc(IngoingArc ingoing)
    {
        if (!ingoing.From.IsCapacityLocation) throw new ArgumentException("The arc did not go from a capacity location!");
        ingoing.ReplaceGuard(ColoredGuard.CapacityGuard(_guardAmount));
    }

    /// <summary>
    /// Appends inhibitor arcs on all locations in _emptyAfter and adds arc fromHat -> transition that consumes n tokens where n
    /// is cap(from) - amountToMove - 1.
    /// </summary>
    /// <param name="transition"></param>
    /// <param name="fromPlaceHat"></param>
    private void AppendArcs(Transition transition, Place fromPlaceHat)
    {
        var emptyAfterMinusFromLocation = _emptyAfter.Where(location => !location.Equals(_fromLocation));
        Location[] afterMinusFromLocation = emptyAfterMinusFromLocation as Location[] ?? emptyAfterMinusFromLocation.ToArray();
        
        InhibitorFromEmptyAfterAdapter inhibitorAdapter =
            new InhibitorFromEmptyAfterAdapter(afterMinusFromLocation);
        inhibitorAdapter.AttachToTransition(transition);

        int weight = _guardAmount;
        var guard = ColoredGuard.CapacityGuard(weight);
        transition.AddInGoingFrom(fromPlaceHat, guard);
    }

    private bool TryGetExistingIngoingFromFromHat(Transition transition, out IngoingArc? arc)
    {
        var arcFromFromHat = transition.InGoing
            .FirstOrDefault(e => e.From.IsCapacityLocation && e.From.IsCreatedFrom(_fromLocation)) ;

        if (arcFromFromHat != null)
        {
            arc = arcFromFromHat;
            return true;
        }
        arc = null;
        return false;
    }
}