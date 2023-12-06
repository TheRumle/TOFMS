using System.Text;
using TACPN.Adapters.TofmToTacpnAdapter;
using Tofms.Common;

namespace Xml;

public class VariableDeclarer
{
    public StringBuilder StringBuilder { get; }

    public VariableDeclarer(StringBuilder stringBuilder)
    {
        StringBuilder = stringBuilder;
    }

    public void WriteVariables(JourneyCollection journeyCollection)
    {
        foreach (var partjour in journeyCollection)
        {
            var part = partjour.Key;
            var journey = partjour.Value.Select(e => e.Value);
            StringBuilder.Append($@"        <variabledecl id=""Var{part}"" name=""{part}var"">
          <usersort declaration=""{Colours.Journey}""/>
            </variabledecl>");


        }
    }
}