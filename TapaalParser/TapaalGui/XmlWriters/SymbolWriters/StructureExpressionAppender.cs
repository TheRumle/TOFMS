using System.Text;
using TACPN.Net;
using TACPN.Net.Places;

namespace TapaalParser.TapaalGui.XmlWriters.SymbolWriters;

public class StructureExpressionAppender
{
    private readonly StringBuilder _builder;

    public StructureExpressionAppender(StringBuilder builder)
    {
        _builder = builder;
    }

    public void AppendStructureText(CapacityPlace place)
    {
    _builder.Append(
            $@"<structure>
              <add>
                <subterm>
                  <numberof>
                    <subterm>
                      <numberconstant value=""{place.Tokens.AmountOfColour("dot")}"">
                        <positive/>
                      </numberconstant>
                    </subterm>
                    <subterm>
                      <useroperator declaration=""dot""/>
                    </subterm>
                  </numberof>
                </subterm>
              </add>
            </structure>");
    }
    
    public void AppendStructureText(Place place)
    {
        _builder.Append(
            $@"<structure>
              <add>
                <subterm>
                  <numberof>
                    <subterm>
                      <numberconstant value=""{0}"">
                        <positive/>
                      </numberconstant>
                    </subterm>
                    <subterm>
                      <useroperator declaration=""dot""/>
                    </subterm>
                  </numberof>
                </subterm>
              </add>
            </structure>");
    }

    private void HandleMultipleColors(TokenCollection tokens)
    {
        _builder.Append(" <structure> <add>");
        foreach (var color in tokens.Colours)
        {
            var n = tokens.AmountOfColour(color);
            var s = $@" <subterm> <numberof> <subterm> <numberconstant value=""{n}""> <positive/> </numberconstant> </subterm> <subterm> <useroperator declaration=""{color}""/> </subterm> </numberof> </subterm>";
            _builder.Append(s);
        }
        
        _builder.Append(@" </add> </structure>" );
    }

    private void HandleSingleColour(string color, int amount)
    {
        _builder.Append(" <structure> <add> ");
        AppendSingleSubTerm(amount, color);
        _builder.Append(" </add> </structure>");
    }

    private void AppendSubTerm(int n, string colour)
    {
        _builder.Append(
            $@" <subterm> <numberof> <subterm> <numberconstant value=""{n}""> <positive/> </numberconstant> </subterm> <subterm> <useroperator declaration=""{colour}""/> </subterm> </numberof> </subterm> ");
    }

    
    private void AppendSingleSubTerm(int n, string colour)
    {
        _builder.Append(
            $@"  <subterm> <numberof> <subterm> <numberconstant value=""{n}""> <positive/> </numberconstant> </subterm> <subterm> <useroperator declaration=""{colour}""/> </subterm> </numberof>  </subterm>");
    }


}