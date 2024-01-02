using TACPN.Net.Arcs;
using TACPN.Net.Colours;
using TACPN.Net.Colours.Expression;
using TACPN.Net.Places;
using TACPN.Net.Transitions;
using Tofms.Common;

namespace TACPN.Adapters.TofmToTacpnAdapter.TransitionAttachable;

internal class ToTransitionAttacher : ITransitionAttachable
{
    public ToTransitionAttacher(Location location,
        IEnumerable<KeyValuePair<string, int>> partsItemMovedIntoPlace, JourneyCollection journeyCollection)
    {
        _itemMovedIntoPlace = partsItemMovedIntoPlace;
        (_place, _capacityPlace) = LocationTranslator.CreatePlaceAndCapacityPlacePair(location, journeyCollection);
    }

    private readonly CapacityPlace _capacityPlace;

    private readonly Place _place;

    private readonly IEnumerable<KeyValuePair<string, int>> _itemMovedIntoPlace;


    public void AttachToTransition(Transition transition)
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
        IColourExpression expression = _place.IsProcessingPlace 
            ? ColourExpressions.PartDecrementExpression(this._itemMovedIntoPlace) 
            : ColourExpressions.MovePartsExpression(this._itemMovedIntoPlace) ;
            
        

        transition.AddOutGoingTo(_place, expression);
    }
}