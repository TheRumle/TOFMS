﻿using TACPN.Net.Arcs;
using TACPN.Net.Colours.Expression;
using TACPN.Net.Colours.Type;
using TACPN.Net.Places;

namespace TACPN.Net.Transitions;

public class Transition
{
    public bool IsCompatibleWith(IPlace other)
    {
        return other.ColourType.Colours.All(e => this.ColourType.Colours.Contains(e));
    }
    
    public ColourType ColourType { get; init; }
    public TransitionGuard TransitionGuard { get; init; }
    
    public Transition(string name, ColourType colourType, TransitionGuard transitionGuard)
    {
        Name = name;
        ColourType = colourType;
        TransitionGuard = transitionGuard;
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

    
    public OutGoingArc AddOutGoingTo(IPlace to, IColourExpression expression)
    {
        var arc = new OutGoingArc(this, to, expression);
        OutGoing.Add(arc);
        return arc;
    }
    
    //TODO add the guards at the right places
    public OutGoingArc AddOutGoingTo(IPlace to, IColourExpression expression, ColourComparison comparison)
    {
        this.TransitionGuard.AddComparison(comparison);
        return this.AddOutGoingTo(to, expression);
    }
    public OutGoingArc AddOutGoingTo(IPlace to, IColourExpression expression, IEnumerable<ColourComparison> comparison)
    {
        this.TransitionGuard.AddComparison(comparison);
        return this.AddOutGoingTo(to, expression);
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
}