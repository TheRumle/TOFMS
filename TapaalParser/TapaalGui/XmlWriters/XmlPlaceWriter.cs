using System.Text;
using System.Text.RegularExpressions;
using TACPN.Colours;
using TACPN.Colours.Type;
using TACPN.Net;
using TACPN.Places;
using TapaalParser.TapaalGui.Placable;
using TapaalParser.TapaalGui.XmlWriters.SymbolWriters;

namespace TapaalParser.TapaalGui.XmlWriters;

public partial class XmlPlaceWriter
{
    private readonly StringBuilder builder;

    public XmlPlaceWriter(StringBuilder builder)
    {
        this.builder = builder;
    }
    public XmlPlaceWriter():this(new StringBuilder()){}
    
     public string XmlString(Placement<Place> placement)
    {
        var element = placement.Construct;
        var name = element.Name;
        var pos = placement.Position;


        builder.Append("<place ");
        AppendPlaceInfo(name, pos);
        AppendColourTypeInfo(element);
        AppendInitialMarking(element);
        
        AppendInvariants(element.ColourInvariants, element.ColourType.Name);
        builder.Append(" </place>");
        return MatchWhiteSpace.Replace(builder.ToString(), " ");
    }


     private string AppendInvariants(IEnumerable<ColourInvariant<int, string>>  elementColorInvariants, string colourType)
    {
        foreach (var kvp in elementColorInvariants)
        {
            var type = colourType;
            if (colourType == ColourType.DefaultColorType.Name)
                type = colourType.ToLower();
            
        }
        return builder.ToString();
    }

    private void AppendInitialMarking(Place place)
    {
        builder.Append("<hlinitialMarking> <text>");
        //TODO use colour expression builder to build initial marking
        
        
        builder.Append("</text>");
        var structureBuilder = new StructureExpressionAppender(builder);
        structureBuilder.AppendStructureText(place);
        builder.Append("  </hlinitialMarking>");
    }

    private void AppendColourTypeInfo(Place element)
    {
        var dcl = element.ColourType.Colours;

        
        //TODO Do structure info for non-capacity place

        builder.Append(
            $@"<type> <text>{dcl}</text> <structure> <usersort declaration=""{dcl}""/> </structure> </type> ");
    }

    private void AppendPlaceInfo(string name, Position pos)
    {
        //TODO Do place info for non-capacity place
    }

    private static Regex MatchWhiteSpace = new Regex("\\s+");

    
    
}