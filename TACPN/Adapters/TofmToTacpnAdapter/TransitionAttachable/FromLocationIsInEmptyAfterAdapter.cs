using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;
using Tofms.Common;

namespace TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;

internal class FromLocationIsInEmptyAfterAdapter : ITransitionAttachable
{
    private readonly Location[] _emptyAfter;
    private readonly Location _fromLocation;
    private readonly int _guardAmount;

    public FromLocationIsInEmptyAfterAdapter(IEnumerable<Location> emptyAfterLocations, Location fromLocation,
        IEnumerable<KeyValuePair<string, int>> partsToConsume)
    {
        _emptyAfter = emptyAfterLocations.ToArray();
        _fromLocation = fromLocation;
        _guardAmount =  fromLocation.Capacity - partsToConsume.Sum(e=>e.Value) - 1;
    }

    public void AttachToTransition(Transition transition)
    {
        if (TryGetExistingInhibitor(transition, out var ingoingFromFromHat))
        {
            ModifyExistingArc(ingoingFromFromHat!);
            return;
        }
        
        var (_, fromPlaceHat) = LocationTranslator.CreateLocationPlacePair(_fromLocation);
        AppendArcs(transition, fromPlaceHat);
    }

    private void ModifyExistingArc(InhibitorArc ingoing)
    {
        if (!ingoing.From.IsCapacityLocation) throw new ArgumentException("The arc did not go from a capacity location!");
        ingoing.Weight = _guardAmount;
    }

    /// <summary>
    /// Appends inhibitor arcs on all locations in emptyAfter and adds arc fromHat -> transition that consumes n tokens where n
    /// is cap(from) - amountToMove - 1.
    /// </summary>
    /// <param name="transition"></param>
    /// <param name="fromPlaceHat"></param>
    private void AppendArcs(Transition transition, Place fromPlaceHat)
    {
        AppendEmptyAfterMinusFrom(transition);
        transition.AddInhibitorFrom(fromPlaceHat, _guardAmount);
    }

    private void AppendEmptyAfterMinusFrom(Transition transition)
    {
        var emptyAfterMinusFromLocation = _emptyAfter
            .Where(location => !location.Equals(_fromLocation));

        InhibitorFromEmptyAfterAdapter inhibitorAdapter = new InhibitorFromEmptyAfterAdapter(emptyAfterMinusFromLocation);
        inhibitorAdapter.AttachToTransition(transition);
    }

    private bool TryGetExistingInhibitor(Transition transition, out InhibitorArc? arc)
    {
        var arcFromFromHat = transition.InhibitorArcs
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