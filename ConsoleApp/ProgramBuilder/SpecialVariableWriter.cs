using System.Text;
using TACPN.Adapters.TofmToTacpnAdapter;
using TACPN.Net;
using TapaalParser.TapaalGui.XmlWriters.Symbols;
using Tofms.Common.JsonTofms.Models;

namespace ConsoleApp.ProgramBuilder;

internal class SpecialVariableWriter
{
    public JourneyCollection JourneyCollection { get; }
    private readonly StringBuilder _builder;

    public SpecialVariableWriter(StringBuilder builder, JourneyCollection journeyCollection)
    {
        JourneyCollection = journeyCollection;
        _builder = builder;
    }



    public void Write()
    {
        foreach (var partAndJourney in this.JourneyCollection)
        {
            var part = partAndJourney.Key;
            var journey = partAndJourney.Value;
            WriteJourney(JourneyCollection.ColourName + part, journey.Count());
        }
        
        WriteTokens();
        WriteVariable();
    }

    private void WriteVariable()
    {
        _builder.Append(
            $@"<variabledcl id=""{GuiSymbols.TokensVariableDclName}"" name=""{GuiSymbols.TokensVariableName}""> <usersort declaration=""{GuiSymbols.TokenVariableColourType}""/> </variabledcl>");
    }

    private void WriteTokens()
    {
        _builder.Append(
            $@"<namedsort id=""{ColourType.TokensColourType.Name}"" name=""{ColourType.TokensColourType.Name}""> <productsort> <usersort declaration=""{TofmJsonSystem.PRODUCTNAME}""/> <usersort declaration=""{JourneyCollection.ColourName}""/> </productsort> </namedsort>");
    }

    private void WriteJourney(string typeName, int maxValue)
    {
        _builder.Append($@"<namedsort id=""{typeName}"" name=""{typeName}""> <finiteintrange end=""{maxValue}"" start=""{0}""/> </namedsort>" );
    }
}