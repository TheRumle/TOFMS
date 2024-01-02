using System.Text;
using TACPN.Net;
using TACPN.Net.Colours;
using TACPN.Net.Colours.Type;
using TapaalParser.TapaalGui.XmlWriters.Symbols;
using Tofms.Common;
using Tofms.Common.JsonTofms.Models;

namespace ConsoleApp.ProgramBuilder;

public class SpecialVariableWriter
{
    private readonly StringBuilder _builder;

    public SpecialVariableWriter(StringBuilder builder)
    {
        _builder = builder;
    }



    public void Write(string typeName, int maxValue)
    {
        WriteJourney(typeName, maxValue);
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