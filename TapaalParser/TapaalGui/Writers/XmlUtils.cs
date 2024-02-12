using Common;
using TACPN.Colours;

namespace TapaalParser.TapaalGui.Writers;

public static class XmlUtils
{
    public static string GetInvariantTagText(ColourInvariant inv)
    {
        return inv.MaxAge == Infteger.PositiveInfinity ? "&lt; inf" : $"&le;= {inv.MaxAge}";
    }
}