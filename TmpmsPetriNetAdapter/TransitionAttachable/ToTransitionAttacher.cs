using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Places;
using TACPN.Transitions;
using Tmpms.Common;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

public class PartColourTupleExpressionFactory
{
    public static List<ColourExpression> CreatePartMoveTuple(IEnumerable<KeyValuePair<string, int>> partsItemMovedIntoPlace, IPlace place )
    {
        var tuples = new List<ColourExpression>();
        foreach (KeyValuePair<string, int> amountAndPart in partsItemMovedIntoPlace)
        {
            IColourTypedValue variableExpression = CreateVariableExpression(place, amountAndPart.Key);
            IEnumerable<IColourValue> values = new[] { new PartColourValue(amountAndPart.Key), variableExpression };
            var tuple = new TupleColour(values,ColourType.TokensColourType);
            tuples.Add(new ColourExpression(tuple, tuple.ColourType, amountAndPart.Value));
        }

        return tuples;
    }

    private static IColourVariableExpression CreateVariableExpression(IPlace place, string part)
    {
        if (place.IsProcessingPlace)
        {
            return ColourVariable.DecrementFor(part);
        }

        return ColourVariable.CreateFromPartName(part, ColourType.PartsColourType);
    }
}

internal class ToTransitionAttacher : ITransitionAttachable
{
    public ToTransitionAttacher(Location toLocation,
        IEnumerable<KeyValuePair<string, int>> partsItemMovedIntoPlace, IndexedJourneyCollection indexedJourneyCollection)
    {
        _itemMovedIntoPlace = partsItemMovedIntoPlace;
        (_place, _capacityPlace) = LocationTranslator.CreatePlaceAndCapacityPlacePair(toLocation, indexedJourneyCollection);
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
        var tuples = PartColourTupleExpressionFactory.CreatePartMoveTuple(this._itemMovedIntoPlace, this._place);
        transition.AddOutGoingTo(_place, tuples);
    }

    

}