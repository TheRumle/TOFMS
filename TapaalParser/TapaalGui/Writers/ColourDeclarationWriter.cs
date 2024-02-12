using TACPN.Colours.Type;
using TACPN.Colours.Values;

namespace TapaalParser.TapaalGui.Writers;


/// <summary>
/// Writes all declarations for colour types and colour variables, but not locations, transitions, etc.
/// </summary>
internal class ColourDeclarationWriter : TacpnUiXmlWriter<(IEnumerable<ColourType> colourTypes, IEnumerable<ColourVariable> variables)>
{
    public ColourDeclarationWriter((IEnumerable<ColourType> colourTypes, IEnumerable<ColourVariable> variables) value) : base(value)
    {
    }

    public override void AppendAllText()
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
            else if (ct is SingletonColourType singletonColourType)
            {
                WriteSingle(singletonColourType);
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
        Append($@"<namedsort id=""{productColourType.Name}"" name=""{productColourType.Name}""> <productsort>");
        Append($@"<usersort declaration=""{productColourType.First.Name}""/>");
        Append($@"<usersort declaration=""{productColourType.Second.Name}""/>");
        Append("</productsort></namedsort>");
    }

    private void WriteColourRange(IntegerRangedColour rangedColour)
    {
        Append($@" <namedsort id=""{rangedColour.Name}"" name=""{rangedColour.Name}"">
                            <finiteintrange end=""{rangedColour.MaxValue}"" start=""0""/>
                           </namedsort>");
    }

    private void WriteMultipleValues(ColourType ct)
    {
        Append($@" <namedsort id=""{ct.Name}"" name=""{ct.Name}""> <cyclicenumeration>");
        foreach (var part in ct.Colours)
        {
            Append($@"<feconstant id=""{part}"" name=""{ct.Name}""/>");
        }
        Append(" </cyclicenumeration> </namedsort>");  
    }

    private void WriteSingle(SingletonColourType ct)
    {
        Append($@" <namedsort id=""{ct.Name}"" name=""{ct.Name}""><{ct.Name}/></namedsort>");
    }
}