using TACPN.Arcs;
using TACPN.Colours.Expression;
using TACPN.Transitions;
using Tmpms.Common;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class EmptyAfterAdapter : ITransitionAttachable
{
    private readonly IEnumerable<Location> _emptyAfterLocations;
    private readonly IndexedJourney _indexedJourney;
    private readonly int _amountsToMove;
    private readonly Location _fromLocation;

    public EmptyAfterAdapter(IEnumerable<Location> emptyAfterLocations, Location fromLocation,
        IEnumerable<KeyValuePair<string, int>> partsToConsume, IndexedJourney collection)
    {
        _emptyAfterLocations = emptyAfterLocations.ToList();
        _fromLocation = fromLocation;
        _amountsToMove = partsToConsume.Sum(e=>e.Value);
        _indexedJourney = collection;
        
    }
    public void AttachToTransition(Transition transition)
    {
        foreach (var place in _emptyAfterLocations.Where(e=>!e.Equals(_fromLocation)).Select(e => LocationTranslator.CreatePlace(e, _indexedJourney)))
            transition.AddInhibitorFrom(place, 1);

        if (_emptyAfterLocations.Contains(_fromLocation) && _amountsToMove.Equals(_fromLocation.Capacity))
        {
            RestoreCapacity(transition);
            DeleteIngoing(transition);
        }
        else if (_emptyAfterLocations.Contains(_fromLocation))
        {
            RestoreCapacity(transition);
            ModifyIngoing(transition);
        } 
    }

    private void DeleteIngoing(Transition transition)
    {
        var fromsCapacityPlace = LocationTranslator.CreateCapacityPlace(_fromLocation);
        var ingoing = transition.InGoing.FirstOrDefault(e => e.From.IsCapacityLocation && e.From.Equals(fromsCapacityPlace));
        if (ingoing is not null)
            transition.InGoing.Remove(ingoing);
    }

    private void ModifyIngoing(Transition transition)   
    {
        var fromsCapacityPlace = LocationTranslator.CreateCapacityPlace(_fromLocation);
        var ingoing = transition.InGoing.FirstOrDefault(e => e.From.IsCapacityLocation && e.From.Equals(fromsCapacityPlace));
        
        if (ingoing is not null) 
            ingoing.SubstituteArcExpressionFor(TimeGuardedArcExpression.CapacityExpression(_fromLocation.Capacity - _amountsToMove));
        else
        {
            transition.AddInGoingFrom(fromsCapacityPlace,_fromLocation.Capacity - _amountsToMove);
        }
        
    }

    private void RestoreCapacity(Transition transition)
    {
        var fromsCapacityPlace = LocationTranslator.CreateCapacityPlace(_fromLocation);
        var outgoing = transition.OutGoing.FirstOrDefault(e => e.To.IsCapacityLocation && e.To.Equals(fromsCapacityPlace));
        if (outgoing is null)
        {
            transition.AddOutGoingTo(fromsCapacityPlace, ColourExpression.CapacityExpression(_fromLocation.Capacity));
        }
        else
        {
            outgoing.SubstituteArcExpressionFor(ColourExpression.CapacityExpression(_fromLocation.Capacity));
        }
    }
}