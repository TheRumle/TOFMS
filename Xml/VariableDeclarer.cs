using System.Text;
using Tmpms.Journey;

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
            StringBuilder.Append($@"<variabledecl id=""{Colours.VariableIdForPart(part)}"" name=""{Colours.VariableNameForPart(part)}"">
          <usersort declaration=""{Colours.Journey}""/>
            </variabledecl>");
        }
    }
}