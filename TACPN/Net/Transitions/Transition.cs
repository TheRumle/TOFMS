using TACPN.Net.Arcs;

namespace TACPN.Net.Transitions;




public class TransitionGuardEqualsString
{
    public string Text { get; set; }
    public TransitionGuardEqualsString(string variable, IEnumerable<int> values)
    {
        this.Variable = variable;
        Text = "";
        var allExceptLast = values.Skip(1);
        foreach (var value in allExceptLast)
        {
            Text += $"{variable} eq {value} or";
        }
        Text += $"{variable} eq {values.First()}";
    }

    public override string ToString()
    {
        return Text;
    }

    public string Variable { get; set; }
}

public class Transition
{
    
    
    public Transition(string name, TransitionGuard guard)
    {
        Name = name;
    }

    public string Name { get; }
    public ICollection<IngoingArc> InGoing { get; } = new List<IngoingArc>();
    public ICollection<OutGoingArc> OutGoing { get; } = new List<OutGoingArc>();
    public ICollection<InhibitorArc> InhibitorArcs { get; } = new List<InhibitorArc>();

    public IngoingArc AddInGoingFrom(Place from, IEnumerable<ColoredGuard> guards)
    {
        var arc = new IngoingArc(from, this, guards);
        InGoing.Add(arc);
        return arc;
    }

    public Arc<IPlace, Transition> AddInGoingFrom(CapacityPlace from, int amount)
    {
        return AddInGoingFrom(from, new[] { ColoredGuard.CapacityGuard(amount) });
    }
    
    
    public IngoingArc AddInGoingFrom(CapacityPlace from, IEnumerable<ColoredGuard> guards)
    {
        var arc = new IngoingArc(from, this, guards);
        InGoing.Add(arc);
        return arc;
    }

    public Arc<IPlace, Transition> AddInGoingFrom(Place from, ColoredGuard guard)
    {
        return AddInGoingFrom(from, new[] { guard });
    }

    


    public OutGoingArc AddOutGoingTo(IPlace from, IEnumerable<Production> productions)
    {
        var arc = new OutGoingArc(this, from, productions);
        OutGoing.Add(arc);
        return arc;
    }

    public Arc<Transition, IPlace> AddOutGoingTo(IPlace from, Production production)
    {
        var arc = new OutGoingArc(this, from, new List<Production> { production });
        OutGoing.Add(arc);
        return arc;
    }

    public InhibitorArc AddInhibitorFrom(IPlace from, int weight)
    {
        var arc = new InhibitorArc(from, this, weight);
        InhibitorArcs.Add(arc);
        return arc;
    }

    public IEnumerable<IPlace> InvolvedPlaces => GetAllPlaces();

    private IEnumerable<IPlace> GetAllPlaces()
    {
        var places = new HashSet<IPlace>();
        places.UnionWith(this.InhibitorArcs.Select(e=>e.From));
        places.UnionWith(this.InGoing.Select(e=>e.From));
        places.UnionWith(this.OutGoing.Select(e=>e.To));
        return places;
    }

    public TransitionGuard Guards { get; set; } = new();

    public void AddGuard(KeyValuePair<int, Place> guardFor)
    {
        this.Guards.Add(guardFor);
    }

    public override string ToString()
    {
        return this.Name;
    }


    public IEnumerable<CapacityPlace> IncomingFromCapacityPlaces()
    {
        return InvolvedPlaces.OfType<CapacityPlace>();
    }
}