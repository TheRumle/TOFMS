using TACPN.Net.Arcs;

namespace TACPN.Net.Transitions;

public class Transition
{
    public Transition(string name)
    {
        Name = name;
    }
    
    public Transition(string name, IEnumerable<IngoingArc> ingoing, IEnumerable<OutGoingArc> outgoing,IEnumerable<InhibitorArc> inhibitorArcs)
    {
        Name = name;
        InhibitorArcs = new List<InhibitorArc>(inhibitorArcs);
        InGoing = new List<IngoingArc>(ingoing);
        OutGoing= new List<OutGoingArc>(outgoing);
    }

    public string Name { get; init; }
    public ICollection<IngoingArc> InGoing { get; } = new List<IngoingArc>();
    public ICollection<OutGoingArc> OutGoing { get; } = new List<OutGoingArc>();
    public ICollection<InhibitorArc> InhibitorArcs { get; } = new List<InhibitorArc>();
    
    public Arc<Place, Transition> AddInGoingFrom(Place from, IEnumerable<ColoredGuard> guards)
    {
        var arc = new IngoingArc(from, this, guards);
        InGoing.Add(arc);
        return arc;
    }
    
    public Arc<Transition, Place> AddOutGoingTo(Place from, IEnumerable<Production> productions)
    {
        var arc = new OutGoingArc(this, from, productions);
        OutGoing.Add(arc);
        return arc;
    }
    
    public Arc<Transition, Place> AddOutGoingTo(Place from, Production production)
    {
        var arc = new OutGoingArc(this, from, new List<Production>(){production});
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


public record Production(string Color, int Amount);
public class OutGoingArc : Arc<Transition, Place>
{
    public readonly IEnumerable<Production> Productions;
    public OutGoingArc(Transition from, Place to, IEnumerable<Production> productions) : base(from, to)
    {
        Productions = productions;
    }
}