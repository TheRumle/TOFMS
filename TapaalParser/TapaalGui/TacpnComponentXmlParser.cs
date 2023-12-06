using System.Text;
using TACPN.Net;
using TACPN.Net.Transitions;
using TapaalParser.TapaalGui.Placable;
using TapaalParser.TapaalGui.XmlWriters;

namespace TapaalParser.TapaalGui;

public class TacpnComponentXmlParser
{
    private readonly IEnumerable<Placement<Transition>> _transitions;
    private readonly  IEnumerable<Placement<Place>> _places;
    private readonly  IEnumerable<Placement<CapacityPlace>> _capacityPlaces;
    private readonly PlacableComponent _component
        ;

    public TacpnComponentXmlParser(PlacableComponent component)
    {
        _places = component.Places;
        _capacityPlaces = component.CapacityPlaces;
        _transitions = component.Transitions;
        _component = component;
    }

    public async Task<string> CreateXmlComponent()
    {
        var places = ConstructPlaces().ContinueWith(CombineStrings);
        var capPlaces = ConstructCapacityPlaces().ContinueWith(CombineStrings);
        var ingoingArcs = ConstructIngoingArcs().ContinueWith(CombineStrings);
        var outgoingArcs = ConstructOutgoingArcs().ContinueWith(CombineStrings);
        var inhibitorArcs = ConstructInhibitorArcs().ContinueWith(CombineStrings);
        var transitions = ConstructTransitions().ContinueWith(CombineStrings);

        var combiner = new StringBuilder();
        WriteTapaalComponentHeader(combiner);
        combiner.Append(await capPlaces);
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

    private async Task<IEnumerable<string>> ConstructInhibitorArcs()
    {
        var arcs = _transitions.SelectMany(e => e.Construct.InhibitorArcs);
        return await ConstructTasks(arcs, e => new InhibitorArcXmlWriter().XmlString(e));
    }

    private async Task<IEnumerable<string>> ConstructOutgoingArcs()
    {
        var outgoings = _transitions.SelectMany(e => e.Construct.OutGoing);
        return await ConstructTasks(outgoings, e => new OutgoingArcXmlWriter().XmlString(e));
    }

    private async Task<IEnumerable<string>> ConstructTransitions()
    {
        return await ConstructTasks(_transitions, t => new TransitionXmlWriter().XmlString(t));
    }

    private async Task<IEnumerable<string>> ConstructIngoingArcs()
    {
        var ingoing = _transitions.SelectMany(e => e.Construct.InGoing);
        return await ConstructTasks(ingoing, i => new IngoingArcXmlWriter().XmlString(i));
    }


    public async Task<IEnumerable<string>> ConstructPlaces()
    {
        return await ConstructTasks(_places, (p) => new XmlPlaceWriter().XmlString(p));
    }
    
    public async Task<IEnumerable<string>> ConstructCapacityPlaces()
    {
        return await ConstructTasks(_capacityPlaces, (p) => new CapacityPlaceXmlWriter().XmlString(p));
    }


    public async Task<IEnumerable<string>> ConstructTasks<T>(IEnumerable<T> source, Func<T, string> xmlWrite)
    {
        
        List<Task<string>> tasks = new List<Task<string>>();
        foreach (var element in source)
            tasks.Add(()=>xmlWrite.Invoke(element));
        
        await Task.WhenAll(tasks);
        return await Task.FromResult(tasks.Select(e=>e.Result));
    }
    
}
