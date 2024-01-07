using TACPN.Net.Arcs;
using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;
using TACPN.Net.Places;
using TACPN.Net.Transitions.Guard;

namespace TACPN.Net.Transitions;



public class Transition
{

    public ColourType ColourType { get; init; }
    public ITransitionGuard TransitionGuard { get; init; }
    
    public Transition(string name, ColourType colourType, ITransitionGuard transitionGuard)
    {
        Name = name;
        ColourType = colourType;
        TransitionGuard = transitionGuard;
    }
    
    public string Name { get; }
    public ICollection<IngoingArc> InGoing { get; } = new List<IngoingArc>();
    public ICollection<OutGoingArc> OutGoing { get; } = new List<OutGoingArc>();
    public ICollection<InhibitorArc> InhibitorArcs { get; } = new List<InhibitorArc>();

    public IngoingArc AddInGoingFrom(IPlace from, IEnumerable<ColourTimeGuard> guards, IColourExpressionAmount expression)
    {
        var arc = new IngoingArc(from, this, guards, Enumerable.Repeat(expression,1));
        InGoing.Add(arc);
        return arc;
    }
    
    public IngoingArc AddInGoingFrom(IPlace from, IEnumerable<ColourTimeGuard> guards, IEnumerable<IColourExpressionAmount> expressions)
    {
        var arc = new IngoingArc(from, this, guards, expressions);
        InGoing.Add(arc);
        return arc;
    }

    public IngoingArc AddInGoingFrom(CapacityPlace from, int amount)
    {
        var expression = ColourExpression.CapacityExpression(amount);
        return AddInGoingFrom(from, new[] { ColourTimeGuard.CapacityGuard()}, expression);
    }

    
    public OutGoingArc AddOutGoingTo(IPlace to, IColourExpressionAmount expression)
    {
        var arc = new OutGoingArc(this, to, expression);
        OutGoing.Add(arc);
        return arc;
    }
    public OutGoingArc AddOutGoingTo(IPlace to, IEnumerable<IColourExpressionAmount> expression)
    {
        var arc = new OutGoingArc(this, to, expression);
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


    public override string ToString()
    {
        return this.Name;
    }
    public IEnumerable<CapacityPlace> IncomingFromCapacityPlaces()
    {
        return InvolvedPlaces.OfType<CapacityPlace>();
    }
    public bool IsCompatibleWith(IPlace other)
    {
        return other.ColourType.Colours.All(e => this.ColourType.Colours.Contains(e));
    }

}