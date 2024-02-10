using TACPN.Colours.Type;
using TACPN.Colours.Values;

namespace TapaalParser.TapaalGui.Writers;

internal class DeclarationsWriter : TacpnUiXmlWriter<(IEnumerable<ColourType> colourTypes, IEnumerable<ColourVariable> variables)>
{
    public DeclarationsWriter((IEnumerable<ColourType> colourTypes, IEnumerable<ColourVariable> variables) value) : base(value)
    {
    }

    public override void AppendToStringBuilder()
    {
        Append($"  <declaration>\n    <structure>\n      <declarations>");
        AppendColourTypes();
        AppendVariables();
        Append($"      </declarations>\n    </structure>\n  </declaration>");
    }

    private void AppendColourTypes()
    {
        foreach (var ct in Parseable.colourTypes)
        {
            if (ct is ProductColourType productColourType)
            {
                WriteProductColour(productColourType);
            }
            else if (ct is IntegerRangedColour rangedColour)
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

    public void AppendVariables()
    {
        foreach (var colourVariable in this.Parseable.variables)
        {
            Append($@"<variabledecl id= ""{colourVariable.Name}"" name= ""{colourVariable.Name}"">
          <usersort declaration=""{colourVariable.ColourType.Name}""/>
            </variabledecl>");
        }
    }

    private void WriteProductColour(ProductColourType productColourType)
    {
        stringBuilder.Append($@"<namedsort id=""{productColourType.Name}"" name=""{productColourType.Name}""> <productsort>");
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