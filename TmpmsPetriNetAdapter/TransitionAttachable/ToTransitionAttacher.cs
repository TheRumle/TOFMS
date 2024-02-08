using TACPN.Colours.Type;
using TACPN.Places;
using TACPN.Transitions;
using Tmpms.Common;
using Tmpms.Common.Journey;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

internal class ToTransitionAttacher : Adapter
{
    private readonly CapacityPlace _capacityPlace;
    private readonly Place _place;
    private readonly IEnumerable<KeyValuePair<string, int>> _itemMovedIntoPlace;

    
    public ToTransitionAttacher(Location toLocation,
        IEnumerable<KeyValuePair<string, int>> partsItemMovedIntoPlace, IndexedJourneyCollection indexedJourneyCollection, ColourType partsColourType) : base(partsColourType, indexedJourneyCollection)
    {
        _itemMovedIntoPlace = partsItemMovedIntoPlace;
        (_place, _capacityPlace) = LocationTranslator.CreatePlaceAndCapacityPlacePair(toLocation, indexedJourneyCollection, partsColourType);
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
        var tuples = this.journeyColourFactory.CreatePartMoveTuple(_itemMovedIntoPlace, this._place, this._collection);
        transition.AddOutGoingTo(_place, tuples);
    }

    

}