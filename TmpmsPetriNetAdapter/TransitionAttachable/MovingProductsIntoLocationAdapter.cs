using TACPN.Colours.Type;
using TACPN.Places;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;
using Tmpms.Common.Move;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class MovingProductsIntoLocationAdapter : Adapter
{
    private readonly CapacityPlace _capacityPlace;
    private readonly Place _place;
    private readonly IEnumerable<KeyValuePair<string, int>> _itemMovedIntoPlace;
    
    public MovingProductsIntoLocationAdapter(MoveAction moveAction, ColourTypeFactory ctFactory, IndexedJourneyCollection collection) 
        : base(ctFactory, collection)
    {
        var placeFactory = new PlaceFactory(ctFactory, collection);
        _itemMovedIntoPlace = moveAction.PartsToMove;
        (_place, _capacityPlace) = LocationTranslator.CreatePlaceAndCapacityPlacePair(moveAction.To, collection, ctFactory.Parts);
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
        var tuples = this.JourneyColourExpressionFactory.CreatePartMoveTuple(_itemMovedIntoPlace, this._place, this.Collection);
        transition.AddOutGoingTo(_place, tuples);
    }

    

}