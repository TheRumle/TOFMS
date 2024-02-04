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

    public void WriteVariables(IndexedJourneyCollection indexedJourneyCollection)
    {
        foreach (var partjour in indexedJourneyCollection)
        {
            var part = partjour.Key;
            var journey = partjour.Value.Select(e => e.Value);
            StringBuilder.Append($@"<variabledecl id=""{Colours.VariableIdForPart(part)}"" name=""{Colours.VariableNameForPart(part)}"">
          <usersort declaration=""{Colours.Journey}""/>
            </variabledecl>");
        }
    }
}