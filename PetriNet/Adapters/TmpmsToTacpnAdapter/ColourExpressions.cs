using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;
using TACPN.Net.Colours.Values;

namespace TACPN.Adapters.TmpmsToTacpnAdapter;

public static class ColourExpressions
{
    
    public static  List<ColourExpression> PartDecrementExpressions(IEnumerable<KeyValuePair<string,int>> partsToMove)
    {
        var expressions = new List<ColourExpression>();
        foreach (var pair in partsToMove)
        {
            var decrementStatement = ColourVariable.DecrementFor(pair.Key);
            expressions.Add(new ColourExpression(decrementStatement, decrementStatement.ColourType, pair.Value));
        }

        return expressions;
    }
    
    
    public static List<ColourExpression> MovePartsExpression(IEnumerable<KeyValuePair<string,int>> partsToMove)
    {
        var expressions = new List<ColourExpression>();
        foreach (var pair in partsToMove)
            expressions.Add(new ColourExpression(new Colour(ColourVariable.VariableNameFor(pair.Key)), ColourType.TokensColourType, pair.Value));

        return expressions;
    }
    
    
}