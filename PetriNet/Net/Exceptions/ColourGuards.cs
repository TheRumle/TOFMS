﻿using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;
using TACPN.Net.Colours.Values;

namespace TACPN.Net.Exceptions;

public static class ColourGuards
{
    public static void MustBeCompatibleWith(this ColourType expressionColourType, IColourValue colourExpressionColourValue)
    {
        if (expressionColourType.IsCompatibleWith(colourExpressionColourValue))
            throw new InvalidColourAssignment(expressionColourType, colourExpressionColourValue);
    }

    public static void MustContain(this ColourType type, IColourValue value)
    {
        if (value is ColourVariable v && v.ColourType != type)
            throw new InvalidColourAssignment(type, v);
        
        if (!type.Colours.Contains(value)) 
            throw new InvalidColourAssignment(type, value);
    }
    
    public static void MustBeAssignableTo(this ColourVariable lhs, int rhs)
    {
        if (!lhs.VariableValues.AsInts.Contains(rhs))
            throw new InvalidColourVariableComparison(lhs, rhs);
    }

}