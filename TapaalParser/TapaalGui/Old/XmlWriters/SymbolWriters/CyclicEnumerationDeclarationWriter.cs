using System.Text;
using TACPN.Colours.Type;

namespace TapaalParser.TapaalGui.Old.XmlWriters.SymbolWriters;

public class CyclicEnumerationDeclarationWriter : IGuiTranslater<IEnumerable<ColourType>>
{
    public string XmlString(IEnumerable<ColourType> colorTypes)
    {
        StringBuilder builder = new StringBuilder();
        var dot = $@" <namedsort id=""dot"" name=""dot""> <dot/></namedsort> "; //DOT must be there
        builder.Append(dot);
        foreach (var colorType in colorTypes)
        {
            if(colorType.Name == ColourType.DefaultColorType.Name) 
                continue;
                
            string dclString = $@" <namedsort id=""{colorType.Name}"" name=""{colorType.Name}"">";
            
            builder.Append(dclString);
            builder.Append(" <cyclicenumeration> ");
            foreach (var color in colorType.Colours)
                builder.Append($@" <feconstant id=""{color}"" name=""{colorType.Name}""/> ");
            builder.Append("  </cyclicenumeration> ");
            builder.Append("</namedsort>");
        }

        return builder.ToString();
    }
    
    
}