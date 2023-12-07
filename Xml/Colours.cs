namespace Xml;

public static class Colours{
    public static readonly string DefaultCapacityColor = "dot";
    public static readonly string Hat = "_capacity";
    public static readonly string TokenColour = "Tokens";
    public static readonly string Parts = "Parts";
    public static readonly string Journey = "Journey";
    public static string VariableNameForPart(string part) => $"{part}var";
    public static string VariableIdForPart(string part) => $"Var{part}";

}