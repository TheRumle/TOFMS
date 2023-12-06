namespace Xml;

public static class Colours{
    public static readonly string DefaultCapacityColor = "dot";
    public static readonly string Hat = "_capacity";
    public static readonly string TokenColour = "Tokens";
    public static readonly string Parts = "Parts";

    public static string JourneyColorForPart(string part) => $"{part}Journey";
}