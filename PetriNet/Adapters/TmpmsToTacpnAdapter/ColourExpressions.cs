using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;
using TACPN.Net.Colours.Values;

namespace TACPN.Adapters.TmpmsToTacpnAdapter;

public static class ColourExpressions
{
    
    public static MultiColourExpression PartDecrementExpression(IEnumerable<KeyValuePair<string,int>> partsToMove)
    {
        var expressions = new List<ColourExpression>();
        foreach (var pair in partsToMove)
        {
            var decrementStatement = Variable.DecrementFor(pair.Key);
            expressions.Add(new ColourExpression(decrementStatement, decrementStatement.ColourType, pair.Value));
        }

        return new MultiColourExpression(expressions, ColourType.TokensColourType);
    }
    
    
    public static MultiColourExpression MovePartsExpression(IEnumerable<KeyValuePair<string,int>> partsToMove)
    {
        var expressions = new List<ColourExpression>();
        foreach (var pair in partsToMove)
            expressions.Add(new ColourExpression(new Colour(Variable.VariableNameFor(pair.Key)), ColourType.TokensColourType, pair.Value));

        return new MultiColourExpression(expressions, ColourType.TokensColourType);
    }
    
    
}