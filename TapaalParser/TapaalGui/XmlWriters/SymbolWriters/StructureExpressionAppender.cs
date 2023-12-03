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
        if (tokens.Colours.Count() == 1)
            HandleSingleColour(tokens);
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

    private void HandleSingleColour(TokenCollection tokens)
    {
       
        foreach (var color in tokens.Colours)
        {
            _builder.Append(" <structure> <add>");
            var n = tokens.AmountOfColour(color);
            AppendSubTerm(n, color);
            _builder.Append(" </add> </structure>");
        }
    }

    private void AppendSubTerm(int n, string colour)
    {
        _builder.Append(
            $@" <subterm> <numberof> <subterm> <numberconstant value=""{n}""> <positive/> </numberconstant> </subterm> <subterm> <useroperator declaration=""{colour}""/> </subterm> </numberof> </subterm> ");
    }


}