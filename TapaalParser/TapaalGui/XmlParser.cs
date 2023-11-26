using System.Collections.Concurrent;
using System.Text;
using TACPN.Net;
using TACPN.Net.Transitions;
using TapaalParser.TapaalGui.Placable;

namespace TapaalParser.TapaalGui;

public class XmlParser : ITapaalGuiParser<PlacableComponent>
{
    private readonly StringBuilder _stringBuilder = new StringBuilder();
    private readonly ParallelQuery<Placement<Transition>> _transitions;
    private readonly  ParallelQuery<Placement<Place>> _places;

    public XmlParser(PlacableComponent component)
    {
        this._places = component.Places.AsParallel();
        this._transitions = component.Transitions.AsParallel();
    }

    public Task<string> CreateXmlComponent()
    {
        var transitionTags = _transitions.AsParallel().Select(ParseTransitions);
        throw new NotImplementedException();
    }

    private string ParseTransitions(Placement<Transition> transition)
    {
        return ";";
    }
    
    
    
    
}

public interface ITapaalGuiParser<T>
{
    public Task<string> CreateXmlComponent();
}