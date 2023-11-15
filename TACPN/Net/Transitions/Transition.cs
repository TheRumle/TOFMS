using TACPN.Net.Arcs;

namespace TACPN.Net.Transitions;

public class Transition
{
    public Transition(string name)
    {
        Name = name;
    }
    
    public Transition(string name, IEnumerable<IngoingArc> ingoing, IEnumerable<Arc<Transition, Place>> outgoing,IEnumerable<InhibitorArc> inhibitorArcs)
    {
        Name = name;
        InhibitorArcs = new List<InhibitorArc>(inhibitorArcs);
        InGoing = new List<IngoingArc>(ingoing);
        OutGoing= new List<Arc<Transition,Place>>(outgoing);
    }

    public string Name { get; init; }
    public ICollection<IngoingArc> InGoing { get; } = new List<IngoingArc>();
    public ICollection<Arc<Transition, Place>> OutGoing { get; } = new List<Arc<Transition, Place>>();
    public ICollection<InhibitorArc> InhibitorArcs { get; } = new List<InhibitorArc>();
    
    public Arc<Place, Transition> AddInGoingFrom(Place from, IEnumerable<ColoredGuard> guards)
    {
        var arc = new IngoingArc(from, this, guards);
        InGoing.Add(arc);
        return arc;
    }
    
    public Arc<Transition, Place> AddOutGoingTo(Place from)
    {
        var arc = new Arc<Transition, Place>(this, from);
        OutGoing.Add(arc);
        return arc;
    }
    
    public InhibitorArc AddInhibitorFrom(Place from, string color)
    {
        InhibitorArc arc = new InhibitorArc(from, this, color);
        this.InhibitorArcs.Add(arc );
        return arc;
    }
    
    public InhibitorArc AddInhibitorFrom(Place from, string color, int amount)
    {
        InhibitorArc arc = new InhibitorArc(from, this, new KeyValuePair<string, int>(color, amount));
        InhibitorArcs.Add(arc );
        return arc;
    }
}