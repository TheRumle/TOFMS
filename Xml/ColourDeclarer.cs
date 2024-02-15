using System.Text;
using Tmpms.Journey;

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

    public void WriteJourney(StringBuilder builder, IndexedJourneyCollection indexedJourneysCollection)
    {
        var maxLength = indexedJourneysCollection.MaxBy(e => e.Value.Count()).Value.Count();
        builder.Append($@" <namedsort id=""Journey"" name=""Journey"">
                            <finiteintrange end=""{maxLength}"" start=""0""/>
                           </namedsort>");
        
    }
    
    public void WriteTokenDeclaration(IndexedJourneyCollection indexedJourneysCollection)
    {

        stringBuilder.Append($@"<namedsort id=""{Colours.TokenColour}"" name=""{Colours.TokenColour}"">
          <productsort>
            <usersort declaration=""{Colours.Parts}""/>");
        
        stringBuilder.Append($@"<usersort declaration=""{Colours.Journey}""/>");
        stringBuilder.Append("</productsort>        </namedsort>");

    }
}