using System.Text;
using TACPN.Net;

namespace TapaalParser.TapaalGui.XmlWriters.SymbolWriters;

public class StructureExpressionAppender
{
    private readonly StringBuilder _builder;

    public StructureExpressionAppender(StringBuilder builder)
    {
        _builder = builder;
    }

    public void AppendStructureText(TokenCollection tokens)
    {
        
        var occurances = tokens.WithMoreThan0Occurances().ToArray();
        if (occurances.Count()== 1)
        {
            var (c, v) = occurances!.First();
            HandleSingleColour(c,v);
        }
        else
        {
            HandleMultipleColors(tokens);
        }
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