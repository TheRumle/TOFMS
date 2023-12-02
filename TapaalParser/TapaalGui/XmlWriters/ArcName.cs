namespace TapaalParser.TapaalGui.XmlWriters;

public static class ArcName
{
    public static readonly string OutPrefix = "Ao";
    public static readonly string InPrefix = "Ai";
    private static int _numOutArcs = 0;
    private static int _numInArcs = 0;

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