using System.Text;
using System.Text.RegularExpressions;
using TACPN.Net;
using TapaalParser.TapaalGui.Placable;
using TapaalParser.TapaalGui.XmlWriters.Symbols;
using TapaalParser.TapaalGui.XmlWriters.SymbolWriters;

namespace TapaalParser.TapaalGui.XmlWriters;

public partial class PlaceXmlWriter : IGuiTranslater<Placement<Place>>
{
    private readonly StringBuilder builder;

    public PlaceXmlWriter(StringBuilder builder)
    {
        this.builder = builder;
    }
    public PlaceXmlWriter():this(new StringBuilder()){}
    
    
    public string XmlString(Placement<Place> placement)
    {
        var element = placement.Construct;
        var name = element.Name;
        var marking = element.Tokens;
        var pos = placement.Position;


        builder.Append("<place ");
        AppendPlaceInfo(name, marking, pos);
        AppendColourTypeInfo(element);
        if (marking.Count > 0)
            AppendInitialMarking(marking);
        
        AppendInvariants(element.ColorInvariants, element.ColourType.Name);
        builder.Append(" </place>");
        return MatchWhiteSpace().Replace(builder.ToString(), " ");
    }

    private string AppendInvariants(IDictionary<string, int> elementColorInvariants, string colourType)
    {
        
        foreach (var kvp in elementColorInvariants)
        {
            var type = colourType;
            if (colourType == ColourType.DefaultColorType.Name)
                type = colourType.ToLower();
            
            InvariantDeclaration decl = InvariantDeclaration.LteInvariant(kvp.Key, kvp.Value, type);
            builder.Append(decl);
        }

        return builder.ToString();
    }

    private void AppendInitialMarking(TokenCollection tokens)
    {
        builder.Append("<hlinitialMarking> <text>");

        var colourExpressionBuilder = new ColorExpressionAppender(builder);
        colourExpressionBuilder.WriteColourExpression(tokens);
        builder.Append("</text>");
        var structureBuilder = new StructureExpressionAppender(builder);
        structureBuilder.AppendStructureText(tokens);
        builder.Append("  </hlinitialMarking>");
    }

    private void AppendColourTypeInfo(Place element)
    {
        var dcl = element.ColourType.Colours.First();
        if (element.ColourType.Colours.Count() > 1)
            dcl = element.ColourType.Name;

        builder.Append(
            $@"<type> <text>{dcl}</text> <structure> <usersort declaration=""{dcl}""/> </structure> </type> ");
    }

    private void AppendPlaceInfo(string name, TokenCollection tokens, Position pos)
    {
        builder.Append($@"displayName=""true"" id=""{name}"" initialMarking=""{tokens.Count}"" invariant=""{GuiSymbols.LessThanInfinity}"" name=""{name}"" nameOffsetX=""0"" nameOffsetY=""0"" positionX=""{pos.X}"" positionY=""{pos.Y}""> ");
    }

    [GeneratedRegex("\\s+")]
    private static partial Regex MatchWhiteSpace();
}