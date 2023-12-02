namespace TapaalParser.TapaalGui.XmlWriters;

public class Comparator
{
    public static string DeclarationSymbol = "&";
    public static Comparator Lt = new Comparator("lt;");
    public static Comparator Lte = new Comparator("lt;=");

    public Comparator(string value)
    {
        Value = $"{DeclarationSymbol}{value}";
    }

    public string Value { get; }

    public static implicit operator string(Comparator symbol)
    {
        return symbol.Value;
    }
}