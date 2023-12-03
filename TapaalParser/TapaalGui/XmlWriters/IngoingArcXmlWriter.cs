using System.Text;
using TACPN.Net;
using TACPN.Net.Arcs;
using TapaalParser.TapaalGui.XmlWriters.Symbols;
using TapaalParser.TapaalGui.XmlWriters.SymbolWriters;

namespace TapaalParser.TapaalGui.XmlWriters;

public class IngoingArcXmlWriter : IGuiTranslater<IngoingArc>
{
    private readonly StringBuilder _builder;

    public IngoingArcXmlWriter(StringBuilder builder)
    {
        _builder = builder;
    }

    public IngoingArcXmlWriter():this(new StringBuilder())
    {
    }
    
    public string XmlString(IngoingArc arc)
    {
        _builder.Append(
            $@"<arc id=""{ArcName.NextIn()}"" inscription=""[0,inf)"" nameOffsetX=""0"" nameOffsetY=""0"" source=""{arc.From.Name}"" target=""{arc.To.Name}"" type=""timed"" weight=""1""> ");
        AppendIntervals(arc);
        AppendHlInscription(arc);
        _builder.Append(" </arc>");        

        return _builder.ToString();

    }

    private void AppendIntervals(IngoingArc arc)
    {
        var intervalDeclarations =
            from colour in arc.From.ColourType.Colours
            from guard in arc.Guards
            where colour == guard.Color
            select new ColorIntervalDeclaration(guard, arc.From.ColourType, colour);

        foreach (var intervalDcl in intervalDeclarations)
            _builder.Append(intervalDcl.ToXmlString());
    }
    
    private void AppendHlInscription(IngoingArc arc)
    {
        IEnumerable<Token> tokens = arc.Guards.SelectMany(e => Enumerable.Repeat(new Token(e.Color), e.Amount));
        TokenCollection collection = new TokenCollection(tokens)
        {
            Colours = arc.From.ColourType.Colours
        };
        
        var expressionBuilder = new ColorExpressionAppender(_builder);
        _builder.Append(@" <hlinscription> <text>");
        expressionBuilder.WriteColourExpression(collection);
        _builder.Append(@"</text>");

        var structureExpressionAppender = new StructureExpressionAppender(_builder);
        structureExpressionAppender.AppendStructureText(collection);
        _builder.Append(" </hlinscription>");
    }

}