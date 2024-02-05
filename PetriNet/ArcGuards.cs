using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Exceptions;
using TACPN.Places;
using TACPN.Transitions;

namespace TACPN;

internal static class ArcGuards{
    public static void InvalidArcColourAssignment(IPlace from, Transition to)
    {
        if (!to.IsCompatibleWith(from)) 
            throw new ArgumentException($"Cannot create arc from {from.Name} ----> {to.Name} because their colours do not match");
    }
    
    public static void InvalidGuardColourAssignment(IPlace place, Transition t, IEnumerable<ColourTimeGuard> guards)
    {
        var coloredGuards = guards as ColourTimeGuard[] ?? guards.ToArray();
        if (coloredGuards.Any(e=>e.ColourType != place.ColourType))
            throw new ArgumentException($"Cannot create arc from {place.Name} ----> {t} with guards {coloredGuards.Select(e=>e + ", ")} because a guard is assigned a different colour type");
    }
    

    public static void InvalidExpressionAssignment(Transition t,IPlace place,  IColourExpression textConvertible)
    {
        if (place.ColourType != textConvertible.ColourType)
            throw new ArgumentException($"Cannot create arc from {t.Name} ----> {place.Name} with arc expression {textConvertible.ExpressionText} because colour types does not match");
    }

    public static void InvalidColourTypeAssignment(IEnumerable<IColourValue> values, ColourType colourType)
    {
        if (!AllColoursMatch(values, colourType))
            throw new InvalidColourAssignment(values, colourType);
    }

    private static bool AllColoursMatch(IEnumerable<IColourValue> values, ColourType colourType)
    {
        var typedElements = values.OfType<IColourTypedValue>();
        var rawColours = values.OfType<Colour>();
        return typedElements.All(v =>
        {
            return colourType.IsCompatibleWith(v);
        }) && rawColours.All(e=>colourType.Colours.Contains(e));
    }
}