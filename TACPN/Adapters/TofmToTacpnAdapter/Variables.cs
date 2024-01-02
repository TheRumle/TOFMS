using TACPN.Net.Colours.Evaluatable;
using TACPN.Net.Colours.Type;

namespace TACPN.Adapters.TofmToTacpnAdapter;

public static class Variables
{
    public static string VariableNameFor(string part)
    {
        return $"Var{part}";
    }
    public static VariableDecrement TokenDecrement = new VariableDecrement("x", ColourType.TokensColourType);

    public static IEnumerable<VariableDecrement> DecrementsFor(IEnumerable<string> parts)
    {
        IEnumerable<VariableDecrement> a = parts.Select(e => new VariableDecrement(VariableNameFor(e), ColourType.TokensColourType));
        return a;
    }
    public static VariableDecrement DecrementFor(string part)
    {
        return new VariableDecrement(VariableNameFor(part), ColourType.TokensColourType);
    }
}