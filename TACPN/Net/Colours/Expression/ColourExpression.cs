using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Expression;

public class ColourExpression : IColourExpression
{
    public IColourExpressionEvaluatable Colour { get; private set; }
    public ColourType ColourType { get; init; }
    public int Amount { get; }

    public ColourExpression(IColourExpressionEvaluatable colour, ColourType colourType, int amount)
    {
        Colour = colour;
        ColourType = colourType;
        Amount = amount;
        Expression = colour.Value;
    }
    
    
    public static ColourExpression CapacityExpression(int amount)
    {
        return new ColourExpression(ColourType.DefaultColorType.Colours.First(), ColourType.DefaultColorType, amount);
    }

    public string Expression { get; }
}