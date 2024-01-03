using System.Text;
using Tmpms.Common;

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
            StringBuilder.Append($@"<variabledecl id=""{Colours.VariableIdForPart(part)}"" name=""{Colours.VariableNameForPart(part)}"">
          <usersort declaration=""{Colours.Journey}""/>
            </variabledecl>");
        }
    }
}