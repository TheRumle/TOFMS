using System.Text;
using TACPN.Net;

namespace TapaalParser.TapaalGui.XmlWriters;

public class ColourDeclarationWriter : IGuiTranslater<IEnumerable<ColourType>>
{
    public string XmlString(IEnumerable<ColourType> colorTypes)
    {
        StringBuilder builder = new StringBuilder();
        var dot = $@" <namedsort id=""dot"" name=""dot""> <dot/></namedsort> "; //DOT must be there
        foreach (var colorType in colorTypes)
        {
            string dclString = $@" <namedsort id=""{colorType.Name}"" name=""{colorType.Name}""><cyclicenumeration> ";
            builder.Append(dclString);
            foreach (var color in colorType.Colours)
                builder.Append($@" <feconstant id=""{color}"" name=""{colorType}""/> ");
            
            builder.Append("</cyclicenumeration> </namedsort> ");
            
        }

        return builder.ToString();
    }
}