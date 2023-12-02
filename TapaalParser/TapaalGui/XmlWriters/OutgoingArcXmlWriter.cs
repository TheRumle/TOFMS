using System.Text;
using TACPN.Net.Arcs;
using TapaalParser.TapaalGui.XmlWriters.Builders;

namespace TapaalParser.TapaalGui.XmlWriters;

public static class ArcName
{
    public static readonly string OutPrefix = "Ao";
    public static readonly string InPrefix = "Ai";
    private static int _numOutArcs = 0;
    private static int _numInArcs = 0;

    public static string NextIn()
    {
        var res = InPrefix + _numInArcs;
        _numInArcs+=1;
        return res;
    }
    
    public static string NextOut()
    {
        var res = OutPrefix + _numOutArcs;
        _numOutArcs+=1;
        return res;
    }
}

public class OutgoingArcXmlWriter : IGuiTranslater<OutGoingArc>
{
    private readonly StringBuilder _builder;

    public OutgoingArcXmlWriter(StringBuilder builder)
    {
        this._builder = builder;
    }

    public OutgoingArcXmlWriter():this(new StringBuilder())
    {
    }
    
    public string XmlString(OutGoingArc arc)
    {
        _builder.Append("<arc ");
        AppendArcInfo(arc);
        AppendHlInscription(arc);
        _builder.Append(" </arc>");
        return _builder.ToString();
    }

    private void AppendHlInscription(OutGoingArc arc)
    {
        var tokens = arc.Productions.ToTokenCollection();
        var expressionBuilder = new ColorExpressionAppender(_builder);
        
        _builder.Append($@" <hlinscription> <text>");
        expressionBuilder.WriteColourExpression(tokens);
        _builder.Append($@"</text>");

        var structureExpressionAppender = new StructureExpressionAppender(_builder);
        structureExpressionAppender.AppendStructureText(tokens);
        _builder.Append($" </hlinscription>");
    }

    private void AppendArcInfo(OutGoingArc arc)
    {
        _builder.Append(
            $@"id=""{ArcName.NextOut()}"" inscription=""1"" nameOffsetX=""-3"" nameOffsetY=""-23"" source=""{arc.From.Name}"" target=""{arc.To.Name}"" type=""normal"" weight=""1"">");
    }
}