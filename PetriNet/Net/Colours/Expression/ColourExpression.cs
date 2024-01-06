﻿using TACPN.Net.Arcs;
using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Expression;

public class ColourExpression : IColourExpressionAmount
{
    public IColourValue ColourValue { get; private set; }
    public ColourType ColourType { get; init; }
    public int Amount { get; set; }

    private ColourExpression(IColourValue colourValue, int amount)
    {
        ExpressionText = $"{amount}'{colourValue.Value}";
        this.Amount = amount;
        this.ColourValue = colourValue;
    }

    public ColourExpression(IColourValue colourValue, ColourType colourType, int amount): this(colourValue, amount)
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

    public ArcExpression AsArcExpression()
    {
        return new ArcExpression(this);
    }
}