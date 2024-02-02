using System.Text;
using TACPN.Arcs;
using TACPN.Colours.Type;
using TapaalParser.TapaalGui.Old.XmlWriters.Symbols;

namespace TapaalParser.TapaalGui.Old.XmlWriters.SymbolWriters;

public class ArcInfoAppender
{
    private readonly StringBuilder _builder;

    public ArcInfoAppender(StringBuilder builder)
    {
        this._builder = builder;
    }
    public void AppendArcInfo(IngoingArc arc)
    {
        var arcType = "timed";
        if (arc.From.ColourType == ColourType.DefaultColorType)
            arcType = "normal";
        _builder.Append(
            $@"<arc id=""{ArcName.NextIn()}"" inscription=""[0,inf)"" nameOffsetX=""0"" nameOffsetY=""0"" source=""{arc.From.Name}"" target=""{arc.To.Name}"" type=""{arcType}"" weight=""1""> ");
    }
    
    public void EndArc(IngoingArc arc)
    {
        _builder.Append(" </arc>");
    }
    
    public void AppendArcInfo(InhibitorArc arc)
    {
        var s = $@"<arc id=""{ArcName.NextInhib()}"" inscription=""[0,inf)"" nameOffsetX=""0"" nameOffsetY=""0"" source=""{arc.From.Name}"" target=""{arc.To.Name}"" type=""tapnInhibitor"" weight=""{arc.Weight}"">";
        _builder.Append(s);
    }
    
        
    public void EndArc(InhibitorArc arc)
    {
        _builder.Append(" </arc>");
    }

    
}