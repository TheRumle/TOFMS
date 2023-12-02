using System.Text;
using TACPN.Net;

namespace TapaalParser.TapaalGui.XmlWriters.Builders;

public class ColorExpressionAppender
{
    private readonly StringBuilder _builder;

    public ColorExpressionAppender(StringBuilder builder)
    {
        _builder = builder;
    }
    
    public void WriteColourExpression(TokenCollection collection)
    {
        var tokens = collection;
        _builder.Append('(');
        foreach (var color in collection.Colours.OrderBy(e=>e))
        {
            var n = tokens.AmountOfColour(color);
            _builder.Append($"{n}'{color} + ");
        }
        if (tokens.Count > 0)
        {
            _builder.Length -= 3;
        }
        _builder.Append(')');
    }
}