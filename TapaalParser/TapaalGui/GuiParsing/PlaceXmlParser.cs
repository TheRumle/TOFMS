using System.Text;
using System.Text.RegularExpressions;
using TACPN.Net;
using TapaalParser.TapaalGui.Placable;

namespace TapaalParser.TapaalGui.GuiParsing;

public class PlaceXmlParser : IGuiTranslater<Place>
{
    public string ToGuiElement(Placement<Place> placement)
    {
        var element = placement.Construct;
        var name = element.Name;
        var tokens = element.Tokens;
        var pos = placement.Position;

        var xmlString = new StringBuilder()
            .Append("<place ")
            .Append(PlaceInfo(name, tokens, pos))
            .Append(ColourTypeInfo(element))
            .Append(CreateInitialMarking(tokens))
            .Append(CreateInvariants(element.ColorInvariants, element.ColourType.Name))
            .Append("</place>").ToString();
        return Regex.Replace(xmlString, @"\s+", " ");
    }

    private string CreateInvariants(IDictionary<string, int> elementColorInvariants, string colourType)
    {
        if (elementColorInvariants.Count == 1) return " ";
        
        var builder = new StringBuilder();
        foreach (var kvp in elementColorInvariants)
        {
            InvariantDeclaration decl = InvariantDeclaration.LteInvariant(kvp.Key, kvp.Value, colourType);
            builder.Append(decl);
        }

        return builder.ToString();
    }

    private string CreateInitialMarking(TokenCollection tokens)
    {
        var builder = new StringBuilder();
        builder.Append("<hlinitialMarking> <text>");
        var colors = tokens.Colours.ToArray();
        
        foreach (var color in colors)
        {
            var n = tokens.AmountOfColour(color);
            builder.Append($"({n}'{color} + ");
        }
        if (tokens.Count > 0)
        {
            builder.Length -= 3;
        }

        builder.Append(')');

        builder.Append("</text> <structure> <add> <subterm> <numberof>");
        
        foreach (var color in colors)
        {
            var n = tokens.AmountOfColour(color);
            builder.Append($" <subterm> <numberconstant value=\"{n}\"> <positive/> </numberconstant> </subterm>");
            builder.Append($" <subterm> <useroperator declaration=\"{color}\"/> </subterm>");
        }
        

        builder.Append(" </numberof> </subterm> </add> </structure> </hlinitialMarking>");
        return builder.ToString();
    }

    private static string ColourTypeInfo(Place element)
    {
        var dcl = element.ColourType.Colours.First();
        if (element.ColourType.Colours.Count() > 1)
            dcl = element.ColourType.Name;
        
        
        return 
            $@"<type> <text>{dcl}</text> <structure> <usersort declaration=""{dcl}""/> </structure> </type> ";
    }

    private static string PlaceInfo(string name, TokenCollection tokens, Position pos)
    {
        var placeInfo = $@"displayName=""true"" id=""{name}"" initialMarking=""{tokens.Count}"" invariant=""{Symbols.LessThanInfinity}"" name=""{name}"" nameOffsetX=""0"" nameOffsetY=""0"" positionX=""{pos.X}"" positionY=""{pos.Y}""> ";
        return placeInfo;
    }
}