namespace TapaalParser.TapaalGui.XmlWriters.Symbols;

public static class ArcName
{
    public static readonly string OutPrefix = "Ao";
    public static readonly string InPrefix = "Ai";
    private static int _numOutArcs;
    private static int _numInArcs;

    public static string NextIn()
    {
        var res = InPrefix + _numInArcs;
        _numInArcs+=1;
        return res;
    }
    
    public static string NextOut()
    {
        var res = OutPrefix + _numOutArcs;
        _numOutArcs+=1;
        return res;
    }
}