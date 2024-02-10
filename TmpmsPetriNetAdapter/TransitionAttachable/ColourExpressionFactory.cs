using TACPN.Colours.Expression;
using TACPN.Colours.Values;
using TACPN.Places;
using Tmpms.Common.Journey;
using TmpmsPetriNetAdapter.Colours;

namespace TmpmsPetriNetAdapter.TransitionAttachable;

public class ColourExpressionFactory
{
    private readonly ColourTypeFactory _colourTypeFactory;
    private readonly ColourVariableFactory _variableFactory;

    public ColourExpressionFactory( ColourTypeFactory ctFactory)
    {
        this._colourTypeFactory = ctFactory;
        _variableFactory = new ColourVariableFactory(ctFactory);
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
            IColourTypedValue variableExpression = CreateVariableExpression(place, amountAndPart.Key);
            IEnumerable<IColourValue> values = [new PartColourValue(_colourTypeFactory.Parts, amountAndPart.Key), variableExpression];
            TupleColour tuple = new TupleColour(values,_colourTypeFactory.Tokens);
            tuples.Add(new ColourExpression(tuple, tuple.ColourType, amountAndPart.Value));
        }

        return tuples;
    }

    private IColourVariableExpression CreateVariableExpression(IPlace place, string part)
    {
        if (place.IsProcessingPlace)
            return _variableFactory.DecrementForPart(part);
        return _variableFactory.VariableForPart(part); 
    }
}