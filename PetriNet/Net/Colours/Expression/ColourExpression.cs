using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Expression;

public class ColourExpression : IColourExpression
{
    public IColourValue Colour { get; private set; }
    public ColourType ColourType { get; init; }
    public int Amount { get; }

    private ColourExpression(IColourValue colour, int amount)
    {
        ExpressionText = $"{amount}'{colour.Value}";
        this.Amount = amount;
        this.Colour = colour;
    }

    public ColourExpression(IColourValue colour, ColourType colourType, int amount): this(colour, amount)
    {
        ColourType = colourType;
    }
    public ColourExpression(IColourTypedValue colour,  int amount): this(colour as IColourValue, amount)
    {
        ColourType = colour.ColourType;
    }
    
    
    public static ColourExpression CapacityExpression(int amount)
    {
        return new ColourExpression(ColourType.DefaultColorType.Colours.First(), ColourType.DefaultColorType, amount);
    }

    public string ExpressionText { get; }
}