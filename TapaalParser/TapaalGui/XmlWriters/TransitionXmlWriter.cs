using System.Text;
using TACPN.Net.Transitions;
using TapaalParser.TapaalGui.Placable;

namespace TapaalParser.TapaalGui.XmlWriters;

public class TransitionXmlWriter : IGuiTranslater<Placement<Transition>>
{
    private readonly StringBuilder _builder;

    public TransitionXmlWriter(StringBuilder builder)
    {
        _builder = builder;
    }

    public TransitionXmlWriter():this(new StringBuilder())
    {
    }
    
    public string XmlString(Placement<Transition> placement)
    {
        var position = placement.Position;
        
        var transition = placement.Construct;
        _builder.Append(@$"<transition angle=""0"" displayName=""true"" id=""{transition.Name}"" infiniteServer=""false"" name=""{transition.Name}"" nameOffsetX=""0"" nameOffsetY=""0"" player=""0"" positionX=""{position.X}"" positionY=""{position.Y}"" priority=""0"" urgent=""false""");
        _builder.Append("/>");

        return _builder.ToString();
    }
}