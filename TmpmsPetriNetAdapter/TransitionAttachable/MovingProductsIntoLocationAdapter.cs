using TACPN.Places;
using TACPN.Transitions;
using Tmpms;
using Tmpms.Journey;
using Tmpms.Move;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class MovingProductsIntoLocationAdapter : Adapter
{
    private readonly Place _capacityPlace;
    private readonly Place _productionPlace;
    private readonly IEnumerable<KeyValuePair<string, int>> _itemMovedIntoPlace;
    private readonly Location to;

    public MovingProductsIntoLocationAdapter(MoveAction moveAction, ColourTypeFactory ctFactory, JourneyCollection collection) 
        : base(ctFactory)
    {
        _itemMovedIntoPlace = moveAction.PartsToMove;
        (_productionPlace, _capacityPlace) = PlaceFactory.CreatePlaceAndCapacityPlacePair(moveAction.To);
        this.to = moveAction.To;
    }

    
    public override void AttachToTransition(Transition transition)
    {
        AdaptPlace(transition);
        AdaptCapacityPlace(transition);
    }

    /// <summary>
    /// Add ingoing for with dots amount of any age for each thing we move
    /// </summary>
    /// <param name="transition"></param>
    private void AdaptCapacityPlace(Transition transition)
    {
        var consumationAmount = _itemMovedIntoPlace.Sum(e => e.Value);
        transition.AddInGoingFrom(_capacityPlace, consumationAmount);
    }

    private void AdaptPlace(Transition transition)
    {
        if (to.IsProcessing)
        {
            var tuples = JourneyColourExpressionFactory.CreatePartJourneyUpdate(_itemMovedIntoPlace);
            transition.AddOutGoingTo(_productionPlace, tuples);
        }
        else
        {
            var tuples = JourneyColourExpressionFactory.CreatePartMoveTuple(_itemMovedIntoPlace);
            transition.AddOutGoingTo(_productionPlace, tuples);
        }
    }

    

}