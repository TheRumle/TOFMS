using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Text;
using TACPN.Adapters.TofmToTacpnAdapter;

namespace Xml;

public class ColourDeclarer
{
    private readonly StringBuilder stringBuilder;

    public ColourDeclarer(StringBuilder builder)
    {
        this.stringBuilder = builder;
    }

    public void WriteParts(IEnumerable<string> Parts)
    {
        stringBuilder.Append($@" <namedsort id=""{Colours.Parts}"" name=""{Colours.Parts}""> <cyclicenumeration>");
        foreach (var part in Parts)
        {
            stringBuilder.Append($@"<feconstant id=""{part}"" name=""{Colours.Parts}""/>");
        }
        stringBuilder.Append(" </cyclicenumeration> </namedsort>");  
    }
    
    public void WriteDot()
    {
        stringBuilder.Append($@" <namedsort id=""{Colours.DefaultCapacityColor}"" name=""{Colours.DefaultCapacityColor}"">
          <dot/></namedsort>");
    }

    public void WriteJourneys(StringBuilder builder, JourneyCollection journeys)
    {
        foreach (var partjour in journeys)
        {
            var part = partjour.Key;
            var journey = partjour.Value.Select(e => e.Value);
            builder.Append($@" <namedsort id=""{part}Journey"" name=""{part}Journey"">
                                <finiteintrange end=""{journey.Count()}"" start=""0""/>
                               </namedsort>");
        }
    }
    
    public void WriteTokenDeclaration(JourneyCollection journeys)
    {

        stringBuilder.Append($@"<namedsort id=""{Colours.TokenColour}"" name=""{Colours.TokenColour}"">
          <productsort>
            <usersort declaration=""{Colours.Parts}""/>");
        
        foreach (var partjour in journeys)
        {
            var part = partjour.Key;
            stringBuilder.Append($@"<usersort declaration=""{Colours.JourneyColorForPart(part)}""/>");
        }

        stringBuilder.Append("</productsort>        </namedsort>");

    }
}