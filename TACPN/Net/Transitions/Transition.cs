using TACPN.Net.Arcs;
using TACPN.Net.Colours;
using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;
using TACPN.Net.Places;

namespace TACPN.Net.Transitions;

public class Transition
{
    public ColourType ColourType { get; init; }
    
    public Transition(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public ICollection<IngoingArc> InGoing { get; } = new List<IngoingArc>();
    public ICollection<OutGoingArc> OutGoing { get; } = new List<OutGoingArc>();
    public ICollection<InhibitorArc> InhibitorArcs { get; } = new List<InhibitorArc>();

    public IngoingArc AddInGoingFrom(IPlace from, IEnumerable<ColoredGuard> guards, IColourExpression expression)
    {
        var arc = new IngoingArc(from, this, guards, expression);
        InGoing.Add(arc);
        return arc;
    }

    public IngoingArc AddInGoingFrom(CapacityPlace from, int amount)
    {
        var expression = ColourExpression.CapacityExpression(amount);
        return AddInGoingFrom(from, new[] { ColoredGuard.CapacityGuard()}, expression);
    }

    
    public OutGoingArc AddOutGoingTo(IPlace from, IColourExpression expression)
    {
        var arc = new OutGoingArc(this, from, expression);
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