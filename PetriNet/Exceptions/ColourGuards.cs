using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Colours.Values;

namespace TACPN.Exceptions;

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
}