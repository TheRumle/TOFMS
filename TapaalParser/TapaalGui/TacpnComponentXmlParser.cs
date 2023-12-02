using System.Text;
using TACPN.Net;
using TACPN.Net.Transitions;
using TapaalParser.TapaalGui.Placable;
using TapaalParser.TapaalGui.XmlWriters;

namespace TapaalParser.TapaalGui;

public class TacpnComponentXmlParser : ITapaalGuiParser<PlacableComponent>
{
    private readonly StringBuilder _stringBuilder = new StringBuilder();
    private readonly IEnumerable<Placement<Transition>> _transitions;
    private readonly  IEnumerable<Placement<Place>> _places;
    private readonly PlacableComponent _component
        ;

    public TacpnComponentXmlParser(PlacableComponent component)
    {
        this._places = component.Places;
        this._transitions = component.Transitions;
        this._component = component;
    }

    public Task<string> CreateXmlComponent()
    {
        Task<IEnumerable<string>> a = ConstructPlacesStrings();
        return Task.FromResult("");
    }

    private string ParseTransitions(Placement<Transition> transition)
    {
        return ";";
    }


    public Task<IEnumerable<string>> ConstructPlacesStrings()
    {
        List<Task<string>> tasks = new List<Task<string>>();

        foreach (var place in _places)
            tasks.Add(() => new PlaceXmlWriter().XmlString(place));
        
        return Task.FromResult(tasks.Select(e=>e.Result));
    }
    
}

public interface ITapaalGuiParser<T>
{
    public Task<string> CreateXmlComponent();
}