using System.Text;
using TACPN.Net;
using TACPN.Net.Transitions;
using TapaalParser.TapaalGui.Placable;
using TapaalParser.TapaalGui.XmlWriters;

namespace TapaalParser.TapaalGui;

public class TacpnComponentXmlParser : ITapaalGuiParser<PlacableComponent>
{
    private readonly IEnumerable<Placement<Transition>> _transitions;
    private readonly  IEnumerable<Placement<Place>> _places;
    private readonly PlacableComponent _component
        ;

    public TacpnComponentXmlParser(PlacableComponent component)
    {
        _places = component.Places;
        _transitions = component.Transitions;
        _component = component;
    }

    public async Task<string> CreateXmlComponent()
    {
        var places = ConstructPlaces().ContinueWith(CombineStrings);
        var ingoingArcs = ConstructIngoingArcs().ContinueWith(CombineStrings);
        var outgoingArcs = ConstructOutgoingArcs().ContinueWith(CombineStrings);
        var inhibitorArcs = ConstructInhibitorArcs().ContinueWith(CombineStrings);
        var transitions = ConstructTransitions().ContinueWith(CombineStrings);
        await Task.WhenAll(places, ingoingArcs, outgoingArcs, inhibitorArcs, transitions);

        var combiner = new StringBuilder();
        WriteTapaalComponentHeader(combiner);
        combiner.Append(await places);
        combiner.Append(await transitions);
        combiner.Append(await ingoingArcs);
        combiner.Append(await outgoingArcs);
        combiner.Append(await inhibitorArcs);

        WriteTapaalComponentFooter(combiner);


        return combiner.ToString();
    }

    private void WriteTapaalComponentFooter(StringBuilder combiner)
    {
        combiner.Append(" </net>");
    }

    private void WriteTapaalComponentHeader(StringBuilder combiner)
    {
        combiner.Append($@" <net active=""true"" id=""{_component.Name}"" type=""P/T net""> ");
    }

    private string CombineStrings(Task<IEnumerable<string>> tasks)
    {
        var combiner = new StringBuilder();
        IEnumerable<string> strings = tasks.Result;
        foreach (var placeString in strings)
            combiner.Append(placeString);
        return combiner.ToString();
    }

    private Task<IEnumerable<string>> ConstructInhibitorArcs()
    {
        var arcs = _transitions.SelectMany(e => e.Construct.InhibitorArcs);
        return ConstructTasks(arcs, e => throw new NotImplementedException("Inhibitor arc parsing not implemented!"));
    }

    private Task<IEnumerable<string>> ConstructOutgoingArcs()
    {
        var outgoings = _transitions.SelectMany(e => e.Construct.OutGoing);
        return ConstructTasks(outgoings, e => new OutgoingArcXmlWriter().XmlString(e));
    }

    private Task<IEnumerable<string>> ConstructTransitions() =>
        ConstructTasks(_transitions, t => new TransitionXmlWriter().XmlString(t));

    private Task<IEnumerable<string>> ConstructIngoingArcs()
    {
        var ingoing = _transitions.SelectMany(e => e.Construct.InGoing);
        return ConstructTasks(ingoing, i => new IngoingArcXmlWriter().XmlString(i));
    }


    public Task<IEnumerable<string>> ConstructPlaces() => ConstructTasks(_places, (p) => new PlaceXmlWriter().XmlString(p));
    
    public Task<IEnumerable<string>> ConstructTasks<T>(IEnumerable<T> source, Func<T, string> xmlWrite)
    {
        
        List<Task<string>> tasks = new List<Task<string>>();
        foreach (var element in source)
            tasks.Add(()=>xmlWrite.Invoke(element));
        
        return Task.FromResult(tasks.Select(e=>e.Result));
    }
    
}

public interface ITapaalGuiParser<T>
{
    public Task<string> CreateXmlComponent();
}