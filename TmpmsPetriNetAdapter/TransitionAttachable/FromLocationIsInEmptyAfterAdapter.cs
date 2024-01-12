using TACPN;
using TACPN.Arcs;
using TACPN.Colours.Expression;
using TACPN.Net;
using TACPN.Places;
using TACPN.Transitions;
using Tmpms.Common;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class FromLocationIsInEmptyAfterAdapter : ITransitionAttachable
{
    private readonly List<Location> _emptyAfter;
    private readonly Location _fromLocation;
    private readonly int _guardAmount;
    private readonly IEnumerable<KeyValuePair<string, int>> _partsToConsume;
    private readonly JourneyCollection _collection;

    public FromLocationIsInEmptyAfterAdapter(IEnumerable<Location> emptyAfterLocations, Location fromLocation,
        IEnumerable<KeyValuePair<string, int>> partsToConsume, JourneyCollection collection)
    {
        _emptyAfter = emptyAfterLocations.ToList();
        _fromLocation = fromLocation;
        _guardAmount = fromLocation.Capacity - partsToConsume.Sum(e=>e.Value);
        _partsToConsume = partsToConsume;
        _collection = collection;
    }

    public void AttachToTransition(Transition transition)
    {
        (Place p, CapacityPlace pcap) = LocationTranslator.CreatePlaceAndCapacityPlacePair(this._fromLocation, this._collection);
        
        CapacityPlace? foundPlace = transition
            .InvolvedPlaces
            .OfType<CapacityPlace>()
            .FirstOrDefault(e => pcap.Equals(e));
        
        if (foundPlace is null)
            foundPlace = pcap;

        ModifyOrAddIncomingFromHat(transition, foundPlace);
        ModifyOrAddOutggoingToHat(transition, foundPlace);

        var alreadyCreated = _emptyAfter
            .Where(l => transition.InvolvedPlaces
                .Any(e => e.IsCreatedFrom(l)));

        _emptyAfter.RemoveAll(e=>alreadyCreated.Contains(e));
        foreach (var location in _emptyAfter)
        {
            transition.AddInhibitorFrom(LocationTranslator.CreatePlace(location, _collection), 1);
        }
    }

    private void ModifyOrAddOutggoingToHat(Transition transition, CapacityPlace fromPlaceHat)
    {
       
        if (TryGetExistingOutgoingFromHat(transition, out var existingOutgoing))
        {
            ModifyExistingArc(existingOutgoing!);
            return;
        }

        transition.AddOutGoingTo(fromPlaceHat, ColourExpression.CapacityExpression(_guardAmount));
    }

    private void ModifyOrAddIncomingFromHat(Transition transition, CapacityPlace fromPlaceHat)
    {
        if (TryGetExistingIngoingFromHat(transition, out var existingIngoing))
        {
            ModifyExistingArc(existingIngoing!);
            return;
        }
        //Append arc
        var amount = _partsToConsume.Sum(e => e.Value);
        transition.AddInGoingFrom(fromPlaceHat, amount);
    }

    private void ModifyExistingArc(IngoingArc ingoing)
    {
        if (!ingoing.From.IsCapacityLocation) throw new ArgumentException("The arc did not go from a capacity location!");
        
        ingoing.TrySetGuardAmount(ColourTimeGuard.CapacityGuard(),_guardAmount);
    }
    
    private void ModifyExistingArc(OutGoingArc arc)
    {
        if (!arc.To.IsCapacityLocation) throw new ArgumentException("The arc did not go from a capacity location!");
        arc.SubstituteArcExpressionFor(ColourExpression.CapacityExpression(_fromLocation.Capacity));
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