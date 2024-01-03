using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Expression;

public class ColourExpression : IColourExpression
{
    public IColourValue Colour { get; private set; }
    public ColourType ColourType { get; init; }
    public int Amount { get; }

    private ColourExpression(IColourValue colour, int amount)
    {
        ExpressionText = $"2'{colour.Value}";
    }

    public ColourExpression(IColourValue colour, ColourType colourType, int amount): this(colour, amount)
    {
        Colour = colour;
        ColourType = colourType;
        Amount = amount;
    }
    public ColourExpression(IColourTypedColourValue colour,  int amount): this(colour as IColourValue, amount)
    {
        Colour = colour;
        ColourType = colour.ColourType;
        Amount = amount;
    }
    
    
    public static ColourExpression CapacityExpression(int amount)
    {
        return new ColourExpression(ColourType.DefaultColorType.Colours.First(), ColourType.DefaultColorType, amount);
    }

    public string ExpressionText { get; }
}