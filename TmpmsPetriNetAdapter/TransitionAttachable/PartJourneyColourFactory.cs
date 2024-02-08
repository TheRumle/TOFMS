using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Places;
using Tmpms.Common;
using Tmpms.Common.Journey;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

public class PartJourneyColourFactory
{
    private readonly ColourType _partsColourType;

    public PartJourneyColourFactory(ColourType partsColourType)
    {
        this._partsColourType = partsColourType;
    }
    
    
    public List<ColourExpression> CreatePartMoveTuple(IEnumerable<KeyValuePair<string, int>> partsItemMovedIntoPlace, IPlace place,
        IndexedJourneyCollection indexedJourney)
    {
        Dictionary<string, int> journeyLengths = indexedJourney.Select(e =>
        {
            return KeyValuePair.Create(
                e.Key,
                e.Value.Count()
            );
        }).ToDictionary();
        
        
        var tuples = new List<ColourExpression>();
        foreach (KeyValuePair<string, int> amountAndPart in partsItemMovedIntoPlace)
        {
            IColourTypedValue variableExpression = CreateVariableExpression(place, amountAndPart.Key, journeyLengths[amountAndPart.Key]);
            IEnumerable<IColourValue> values = [new PartColourValue(_partsColourType, amountAndPart.Key), variableExpression];
            TupleColour tuple = new TupleColour(values,ColourType.TokensColourType(_partsColourType));
            tuples.Add(new ColourExpression(tuple, tuple.ColourType, amountAndPart.Value));
        }

        return tuples;
    }

    private static IColourVariableExpression CreateVariableExpression(IPlace place, string part, int journeyLength)
    {
        if (place.IsProcessingPlace)
            return ColourVariable.DecrementForPartType(part,journeyLength);

        return ColourVariable.CreateFromPartName(part, journeyLength);
    }
}