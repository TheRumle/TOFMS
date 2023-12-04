using System.Text;
using TACPN.Net;

namespace TapaalParser.TapaalGui.XmlWriters.SymbolWriters;

public class ColorExpressionAppender
{
    private readonly StringBuilder _builder;

    public ColorExpressionAppender(StringBuilder builder)
    {
        _builder = builder;
    }
    
    public void WriteColourExpression(TokenCollection collection)
    {
        _builder.Append('(');
        var tokens = collection.WithMoreThan0Occurances().ToArray();

        if (tokens.Length > 1)
            AppendColourExpressions(tokens);
        else
            _builder.Append($"{tokens.First().Amount}'{tokens.First().Color}");
        
        _builder.Append(')');
    }

    private void AppendColourExpressions((string Color, int Amount)[] tokens)
    {
        var last = tokens.Last();
        var elementsExceptLast = tokens.Take(tokens.Length - 1);
        foreach (var colorAmount in elementsExceptLast)
            _builder.Append($"{colorAmount.Amount}'{colorAmount.Color} + ");


        _builder.Append($"{last.Amount}'{last.Color}");
    }
}