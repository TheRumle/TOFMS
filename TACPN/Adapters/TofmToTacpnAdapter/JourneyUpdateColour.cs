using TACPN.Net.Colours.Evaluatable;
using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;

namespace TACPN.Adapters.TofmToTacpnAdapter;

public static class ColourExpressions
{
    public static CombinedColourExpression PartDecrementExpression(IEnumerable<KeyValuePair<string,int>> partsToMove)
    {
        var expressions = new List<ColourExpression>();
        foreach (var pair in partsToMove)
        {
            var decrementStatement = Variables.DecrementFor(pair.Key);
            expressions.Add(new ColourExpression(decrementStatement, decrementStatement.ColourType, pair.Value));
        }

        return new CombinedColourExpression(expressions, ColourType.TokensColourType);
    }
    
    
    public static CombinedColourExpression MovePartsExpression(IEnumerable<KeyValuePair<string,int>> partsToMove)
    {
        var expressions = new List<ColourExpression>();
        foreach (var pair in partsToMove)
            expressions.Add(new ColourExpression(new Colour(Variables.VariableNameFor(pair.Key)), ColourType.TokensColourType, pair.Value));

        return new CombinedColourExpression(expressions, ColourType.TokensColourType);
    }
}