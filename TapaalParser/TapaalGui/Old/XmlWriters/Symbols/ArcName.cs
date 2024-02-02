namespace TapaalParser.TapaalGui.Old.XmlWriters.Symbols;

public static class ArcName
{
    public static readonly string OutPrefix = "Ao";
    public static readonly string InPrefix = "Ai";
    public static readonly string InhibPrefix = "I";
    private static int _numOutArcs;
    private static int _numInArcs;
    private static int _numInhibArcs;

    public static string NextInhib()
    {
        var res = InhibPrefix + _numInhibArcs;
        _numInhibArcs+=1;
        return res;
    }
    
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