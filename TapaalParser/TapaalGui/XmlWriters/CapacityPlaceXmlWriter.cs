using System.Text;
using System.Text.RegularExpressions;
using TACPN;
using TACPN.Colours;
using TACPN.Colours.Type;
using TACPN.Net;
using TACPN.Places;
using TapaalParser.TapaalGui.Placable;
using TapaalParser.TapaalGui.XmlWriters.Symbols;
using TapaalParser.TapaalGui.XmlWriters.SymbolWriters;

namespace TapaalParser.TapaalGui.XmlWriters;

public partial class CapacityPlaceXmlWriter : IGuiTranslater<Placement<CapacityPlace>>
{
    private readonly StringBuilder builder;

    public CapacityPlaceXmlWriter(StringBuilder builder)
    {
        this.builder = builder;
    }
    public CapacityPlaceXmlWriter():this(new StringBuilder()){}
    
    
    public string XmlString(Placement<CapacityPlace> placement)
    {
        var element = placement.Construct;
        var name = element.Name;
        var marking = element.Tokens;
        var pos = placement.Position;


        builder.Append("<place ");
        AppendPlaceInfo(name, marking, pos);
        AppendColourTypeInfo(element);
        if (marking.Count > 0)
            AppendInitialMarking(marking, element);
        
        AppendInvariants(element.ColourInvariants, element.ColourType.Name);
        builder.Append(" </place>");
        return MatchWhiteSpace.Replace(builder.ToString(), " ");
    }
    
    
    private void AppendInvariants(IEnumerable<ColourInvariant<string>> elementColorInvariants, string colourType)
    {
        foreach (var kvp in elementColorInvariants)
        {
            var type = colourType;
            if (colourType == ColourType.DefaultColorType.Name)
                type = colourType.ToLower();
            
            InvariantDeclaration decl = InvariantDeclaration.LteInvariant(kvp.FirstColour, kvp.MaxAge, type);
            builder.Append(decl);
        }
    }

    private void AppendInitialMarking(TokenCollection tokens, CapacityPlace place)
    {
        builder.Append("<hlinitialMarking> <text>");

        var colourExpressionBuilder = new ColorExpressionAppender(builder);
        colourExpressionBuilder.WriteColourExpression(tokens);
        builder.Append("</text>");
        var structureBuilder = new StructureExpressionAppender(builder);
        structureBuilder.AppendStructureText(place);
        builder.Append("  </hlinitialMarking>");
    }

    private void AppendColourTypeInfo(CapacityPlace element)
    {
        var dcl = element.ColourType.Colours.First().Value;
        if (element.ColourType.Colours.Count() > 1)
            dcl = element.ColourType.Name;

        builder.Append(
            $@"<type> <text>{dcl}</text> <structure> <usersort declaration=""{dcl}""/> </structure> </type> ");
    }

    private void AppendPlaceInfo(string name, TokenCollection tokens, Position pos)
    {
        builder.Append($@"displayName=""true"" id=""{name}"" initialMarking=""{tokens.Count}"" invariant=""{GuiSymbols.LessThanInfinity}"" name=""{name}"" nameOffsetX=""0"" nameOffsetY=""0"" positionX=""{pos.X}"" positionY=""{pos.Y}""> ");
    }

    private static Regex MatchWhiteSpace = new Regex("\\s+");
}