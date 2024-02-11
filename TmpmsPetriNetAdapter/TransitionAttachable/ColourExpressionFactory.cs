using TACPN.Colours.Expression;
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
    
    
    public IEnumerable<ColourExpression> CreatePartMoveTuple(IEnumerable<KeyValuePair<string, int>> partsItemMovedIntoPlace)
    {
        foreach (KeyValuePair<string, int> amountAndPart in partsItemMovedIntoPlace)
            yield return CreateExpressionWith(amountAndPart, partType => _variableFactory.VariableForPart(partType));
    }
    
    public IEnumerable<ColourExpression> CreatePartJourneyUpdate(IEnumerable<KeyValuePair<string, int>> partsItemMovedIntoPlace)
    {
        foreach (KeyValuePair<string, int> amountAndPart in partsItemMovedIntoPlace)
            yield return CreateExpressionWith(amountAndPart, partType => _variableFactory.DecrementForPart(partType));

    }

    private ColourExpression CreateExpressionWith(KeyValuePair<string, int> amountAndPart, Func<string, IColourTypedValue> factory)
    {
        IColourTypedValue variableExpression = factory.Invoke(amountAndPart.Key);
        IEnumerable<IColourValue> values = [new PartColourValue(_colourTypeFactory.Parts, amountAndPart.Key), variableExpression];
        TupleColour tuple = new TupleColour(values,_colourTypeFactory.Tokens);
        return new ColourExpression(tuple, tuple.ColourType, amountAndPart.Value);
    }
}