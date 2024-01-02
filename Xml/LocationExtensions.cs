using Tofms.Common;

namespace Xml;

public static class LocationExtensions
{
    public static CapacityLocation ToCapacityLocation(this Location location)
    {
        return new CapacityLocation(location.Name, location.Capacity);
    }
    
    public static Invariant Infinity = new Invariant("Dot", 0, Infteger.PositiveInfinity);
}

public static class SymbolExtensions {
    public static string ToSymbol(this int value)
    {
        if (value == Infteger.PositiveInfinity) return "inf";
        return value.ToString();
    }
    
    
    public static string ToInvariantText(this Invariant value)
    {
        if (value.Max == Infteger.PositiveInfinity) return $"[{value.Min},inf)";
        return $"[{value.Min},{value.Max}]";
    }
    
    
}