using TACPN.Net.Colours.Evaluatable;
using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;
using TACPN.Net.Places;
using TACPN.Net.Transitions;

namespace TACPN.Net;

internal static class GuardFrom{
    public static void InvalidArcColourAssignment(IPlace from, Transition to)
    {
        if (!to.IsCompatibleWith(from)) 
            throw new ArgumentException($"Cannot create arc from {from.Name} ----> {to.Name} because their colours do not match");
    }
    
    public static void InvalidGuardColourAssignment(IPlace place, Transition t, IEnumerable<ColoredGuard> guards)
    {
        var coloredGuards = guards as ColoredGuard[] ?? guards.ToArray();
        
        if (coloredGuards.Any(e=>e.ColourType != place.ColourType))
            throw new ArgumentException($"Cannot create arc from {place.Name} ----> {t} with guards {coloredGuards.Select(e=>e + ", ")} because a guard is assigned a different colour type");
    }

    public static void InvalidExpressionAssignment(IPlace place, Transition t, IColourExpression expression)
    {
        if (!t.IsCompatibleWith(place) || place.ColourType != expression.ColourType)
            throw new ArgumentException($"Cannot create arc from {place.Name} ----> {t.Name} with arc expression {expression.ExpressionText}");
    }
    
    public static void InvalidArcColourAssignment(Transition transition, IPlace place)
    {
        if (transition.ColourType != place.ColourType) 
            throw new ArgumentException($"Cannot create arc from {transition.Name} ----> {place.Name} because their colours do not match");
    }
    
    public static void InvalidGuardColourAssignment( Transition t, IPlace place, IEnumerable<ColoredGuard> guards)
    {
        var coloredGuards = guards as ColoredGuard[] ?? guards.ToArray();
        
        if (coloredGuards.Any(e=>e.ColourType != place.ColourType))
            throw new ArgumentException($"Cannot create arc from  {t.Name} ----> {place.Name} with guards {coloredGuards.Select(e=>e + ", ")} because a guard is assigned a different colour type does not match");
    }

    public static void InvalidExpressionAssignment(Transition t,IPlace place,  IColourExpression expression)
    {
        if (place.ColourType != expression.ColourType)
            throw new ArgumentException($"Cannot create arc from {t.Name} ----> {place.Name} with arc expression {expression.ExpressionText} because colour types does not match");
    }

    public static void InvalidExpressionAssignment(IEnumerable<IColourExpression> subExpressions, ColourType colourType)
    {
        if (subExpressions.Any(e=>e.ColourType != colourType))
            throw new ArgumentException($"Cannot create colour expression with colourtype {colourType.Name} from expressions {subExpressions.Select(e=>e + "  ")} because at least one subexpression has a different colour type");

    }
    
    public static void InvalidColourTypeAssignment(IEnumerable<IColourValue> values, ColourType colourType)
    {
        //Get all values that are not of type variable, variableDecrement, variableIncrement
        if (!AllColoursMatch(values, colourType) )
            throw new ArgumentException($@"Cannot make assignment of colour values ""{string.Join(", ", values.Select(e=>e.Value))}"" with colour type {colourType.Name} because at least one of the colours does not belong to the colour type");

    }

    private static bool AllColoursMatch(IEnumerable<IColourValue> values, ColourType colourType)
    {
        var typedElements = values.OfType<IColourTypedColourValue>();
        var rawColours = values.OfType<Colour>();
        return typedElements.All(v => v.ColourType == colourType) && rawColours.All(e=>colourType.Colours.Contains(e));
    }
}