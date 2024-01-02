﻿using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Expression;

public class ColourExpression : IColourExpression
{
    public IColourExpressionEvaluatable Colour { get; private set; }
    public ColourType ColourType { get; init; }
    public int Amount { get; }
    public IEnumerable<IColourExpression> AsAtomicValues()
    {
        return new[] { this };
    }

    public ColourExpression(IColourExpressionEvaluatable colour, ColourType colourType, int amount)
    {
        Colour = colour;
        ColourType = colourType;
        Amount = amount;
        ExpressionText = colour.Value;
    }
    
    
    public static ColourExpression CapacityExpression(int amount)
    {
        return new ColourExpression(ColourType.DefaultColorType.Colours.First(), ColourType.DefaultColorType, amount);
    }

    public string ExpressionText { get; }
}