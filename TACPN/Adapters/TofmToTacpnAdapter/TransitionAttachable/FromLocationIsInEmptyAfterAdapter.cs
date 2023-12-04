using TACPN.Net;
using TACPN.Net.Arcs;
using TACPN.Net.Transitions;
using Tofms.Common;

namespace TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;

internal class FromLocationIsInEmptyAfterAdapter : ITransitionAttachable
{
    private readonly List<Location> _emptyAfter;
    private readonly Location _fromLocation;
    private readonly int _guardAmount;
    private readonly IEnumerable<KeyValuePair<string, int>> _partsToConsume;

    public FromLocationIsInEmptyAfterAdapter(IEnumerable<Location> emptyAfterLocations, Location fromLocation,
        IEnumerable<KeyValuePair<string, int>> partsToConsume)
    {
        _emptyAfter = emptyAfterLocations.ToList();
        _fromLocation = fromLocation;
        _guardAmount = fromLocation.Capacity - partsToConsume.Sum(e=>e.Value);
        _partsToConsume = partsToConsume;
    }

    public void AttachToTransition(Transition transition)
    {
        (Place p, Place pcap) = LocationTranslator.CreatePlaceAndCapacityPlacePair(this._fromLocation);
        var existingCapPlace = transition
            .InvolvedPlaces
            .FirstOrDefault(e => pcap.Equals(e));
        
        if (existingCapPlace is null)
            existingCapPlace = pcap;
        
        ModifyOrAddIncomingFromHat(transition, existingCapPlace);
        ModifyOrAddOutggoingToHat(transition, existingCapPlace);

        var alreadyCreated = _emptyAfter
            .Where(l => transition.InvolvedPlaces
                .Any(e => e.IsCreatedFrom(l)));

        _emptyAfter.RemoveAll(e=>alreadyCreated.Contains(e));
        foreach (var location in _emptyAfter)
        {
            transition.AddInhibitorFrom(LocationTranslator.CreatePlace(location), 1);
        }

    }

    private void ModifyOrAddOutggoingToHat(Transition transition, Place fromPlaceHat)
    {
       
        if (TryGetExistingOutgoingFromHat(transition, out var existingOutgoing))
        {
            ModifyExistingArc(existingOutgoing!);
            return;
        }

        transition.AddOutGoingTo(fromPlaceHat, new Production(ColourType.DefaultColorType.Name, _guardAmount));
    }

    private void ModifyOrAddIncomingFromHat(Transition transition, Place fromPlaceHat)
    {
        if (TryGetExistingIngoingFromHat(transition, out var existingIngoing))
        {
            ModifyExistingArc(existingIngoing!);
            return;
        }
        //Append arc
        transition.AddInGoingFrom(fromPlaceHat, ColoredGuard.CapacityGuard(_partsToConsume.Sum(e => e.Value)));
    }

    private void ModifyExistingArc(IngoingArc ingoing)
    {
        if (!ingoing.From.IsCapacityLocation) throw new ArgumentException("The arc did not go from a capacity location!");
        ingoing.ReplaceGuard(ColoredGuard.CapacityGuard(_guardAmount));
    }
    
    private void ModifyExistingArc(OutGoingArc arc)
    {
        if (!arc.To.IsCapacityLocation) throw new ArgumentException("The arc did not go from a capacity location!");
        
        arc.Productions = new List<Production>()
        {
            new(CapacityPlaceExtensions.DefaultCapacityColor, _fromLocation.Capacity)
        };
    }

    private bool TryGetExistingIngoingFromHat(Transition transition, out IngoingArc? arc)
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
    
    private bool TryGetExistingOutgoingFromHat(Transition transition, out OutGoingArc? arc)
    {
        var arcToHat = transition.OutGoing
            .FirstOrDefault(e => e.To.IsCapacityLocation && e.To.IsCreatedFrom(
                _fromLocation));

        if (arcToHat != null)
        {
            arc = arcToHat;
            return true;
        }
        arc = null;
        return false;
    }
}