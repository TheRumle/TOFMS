using TACPN.Arcs;
using TACPN.Colours.Expression;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class EmptyAfterAdapter :Adapter
{
    private readonly IEnumerable<Location> _emptyAfterLocations;
    private readonly int _amountsToMove;
    private readonly Location _fromLocation;
    
    public EmptyAfterAdapter(MoveAction moveAction, ColourTypeFactory ctFactory, IndexedJourneyCollection collection) : base(ctFactory, collection)
    {
        _emptyAfterLocations = moveAction.EmptyAfter;
        _fromLocation = moveAction.From;
        _amountsToMove = moveAction.PartsToMove.Sum(e=>e.Value);
    }
    public override void AttachToTransition(Transition transition)
    {
        foreach (var place in _emptyAfterLocations.Where(e => !e.Equals(_fromLocation))
                     .Select(PlaceFactory.CreatePlace))
        {
            transition.AddInhibitorFrom(place, 1);
        }

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
        var fromsCapacityPlace = PlaceFactory.CreateInitializedCapacityPlaceFor(_fromLocation);
        var ingoing = transition.InGoing.FirstOrDefault(e => e.From.IsCapacityLocation && e.From.Equals(fromsCapacityPlace));
        if (ingoing is not null)
            transition.InGoing.Remove(ingoing);
    }

    private void ModifyIngoing(Transition transition)   
    {
        var fromsCapacityPlace = PlaceFactory.CreateInitializedCapacityPlaceFor(_fromLocation);
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
        var fromsCapacityPlace = PlaceFactory.CreateInitializedCapacityPlaceFor(_fromLocation);
        var outgoing = transition.OutGoing.FirstOrDefault(e => e.To.IsCapacityLocation && e.To.Equals(fromsCapacityPlace));
        if (outgoing is null)
        {
            transition.AddOutGoingTo(fromsCapacityPlace, ColourExpression.DefaultTokenExpression(_fromLocation.Capacity));
        }
        else
        {
            outgoing.SubstituteArcExpressionFor(ColourExpression.DefaultTokenExpression(_fromLocation.Capacity));
        }
    }
}