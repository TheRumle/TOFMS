using System.Text;
using TACPN.Arcs;
using TACPN.Net;
using TACPN.Places;
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
        AppendIntervals(arc); //This needs ] -> ) TODo
        AppendHlInscription(arc);
        _builder.Append(" </arc>");        

        return _builder.ToString();

    }

    private void AppendIntervals(IngoingArc arc)
    {
        IEnumerable<ColorIntervalDeclaration> intervalDeclarations =
            from colour in arc.From.ColourType.Colours
            from guard in arc.Guards
            where colour == guard.ColourType.Name
            select new ColorIntervalDeclaration(guard, arc.From.ColourType, colour);

        foreach (var intervalDcl in intervalDeclarations)
            _builder.Append(intervalDcl.ToXmlString());
    }
    
    private void AppendHlInscription(IngoingArc arc)
    {
        
        
        var expressionBuilder = new ColorExpressionAppender(_builder);
        _builder.Append(@" <hlinscription> <text>");
        if (arc.From is CapacityPlace cap)
        {
            var a = arc.Guards;
        }
        
        
        _builder.Append(@"</text>");

        var structureExpressionAppender = new StructureExpressionAppender(_builder);
        _builder.Append(" </hlinscription>");
    }
    
    

}