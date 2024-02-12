using TACPN.Arcs;
using TACPN.Colours.Expression;
using TACPN.Colours.Type;
using TACPN.Places;
using TACPN.Transitions.Guard;

namespace TACPN.Transitions;


public class Transition
{
    public ColourType ColourType { get; init; }
    public ITransitionGuard Guard { get; init; }
    
    public Transition(string name, ColourType colourType, ITransitionGuard guard)
    {
        Name = name;
        ColourType = colourType;
        Guard = guard;
    }
    
    public string Name { get; }
    public ICollection<IngoingArc> InGoing { get; } = new List<IngoingArc>();
    public ICollection<OutGoingArc> OutGoing { get; } = new List<OutGoingArc>();
    public ICollection<InhibitorArc> InhibitorArcs { get; } = new List<InhibitorArc>();

    public IngoingArc AddInGoingFrom(Place from, IEnumerable<ColourTimeGuard> guards, IColourExpressionAmount expression)
    {
        var arc = new IngoingArc(from, this, guards, Enumerable.Repeat(expression,1));
        InGoing.Add(arc);
        return arc;
    }
    
    public IngoingArc AddInGoingFrom(Place from, IEnumerable<ColourTimeGuard> guards, IEnumerable<IColourExpressionAmount> expressions)
    {
        var arc = new IngoingArc(from, this, guards, expressions);
        InGoing.Add(arc);
        return arc;
    }
    
    public IngoingArc AddInGoingFrom(Place from, int amount)
    {
        var expression = ColourExpression.DefaultTokenExpression(amount);
        return AddInGoingFrom(from, new[] { ColourTimeGuard.Default()}, expression);
    }

    
    public OutGoingArc AddOutGoingTo(Place to, IColourExpressionAmount expression)
    {
        var arc = new OutGoingArc(this, to, expression);
        OutGoing.Add(arc);
        return arc;
    }
    public OutGoingArc AddOutGoingTo(Place to, IEnumerable<IColourExpressionAmount> expression)
    {
        var arc = new OutGoingArc(this, to, expression);
        OutGoing.Add(arc);
        return arc;
    }
    

    public InhibitorArc AddInhibitorFrom(Place from, int weight)
    {
        var arc = new InhibitorArc(from, this, weight);
        InhibitorArcs.Add(arc);
        return arc;
    }

    public IEnumerable<Place> InvolvedPlaces => GetAllPlaces();

    private IEnumerable<Place> GetAllPlaces()
    {
        var places = new HashSet<Place>();
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
    public bool IsCompatibleWith(Place other)
    {
        return other.ColourType
            .Colours
            .All(e => this.ColourType.Colours.Contains(e));
    }

}