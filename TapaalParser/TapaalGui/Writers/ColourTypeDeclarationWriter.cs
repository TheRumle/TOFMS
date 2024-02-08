using System.Security.Cryptography.X509Certificates;
using TACPN.Colours.Type;

namespace TapaalParser.TapaalGui.Writers;

internal class ColourTypeDeclarationWriter : TacpnUiXmlWriter<IEnumerable<ColourType>>
{
    public ColourTypeDeclarationWriter(IEnumerable<ColourType> value) : base(value.DistinctBy(e=>e.Name))
    {
        
    }

    public override void AppendToStringBuilder()
    {
        foreach (var ct in Parseable)
        {
            if (ct is ProductColourType productColourType)
            {
                WriteProductColour(productColourType);


            }else if (ct is IntegerRangedColour rangedColour)
            {
                WriteColourRange(rangedColour);
            }
            else if (ct.Colours.Count == 1)
            {
                WriteSingle(ct);
            }
            else
            {
                WriteMultipleValues(ct);
            }

        }
    }

    private void WriteProductColour(ProductColourType productColourType)
    {
        stringBuilder.Append($@"<namedsort id=""{productColourType}"" name=""{productColourType}""> <productsort>");
        stringBuilder.Append($@"<usersort declaration=""{productColourType.First.Name}""/>");
        stringBuilder.Append($@"<usersort declaration=""{productColourType.Second.Name}""/>");
        stringBuilder.Append("</productsort></namedsort>");
    }

    private void WriteColourRange(IntegerRangedColour rangedColour)
    {
        stringBuilder.Append($@" <namedsort id=""{rangedColour.Name}"" name=""{rangedColour.Name}"">
                            <finiteintrange end=""{rangedColour.MaxValue}"" start=""0""/>
                           </namedsort>");
    }

    private void WriteMultipleValues(ColourType ct)
    {
        stringBuilder.Append($@" <namedsort id=""{ct.Name}"" name=""{ct.Name}""> <cyclicenumeration>");
        foreach (var part in ct.Colours)
        {
            stringBuilder.Append($@"<feconstant id=""{part}"" name=""{ct.Name}""/>");
        }
        stringBuilder.Append(" </cyclicenumeration> </namedsort>");  
    }

    private void WriteSingle(ColourType ct)
    {
        stringBuilder.Append($@" <namedsort id=""{ct.Name}"" name=""{ct.Name}""><{ct.Name}/></namedsort>");
    }
}