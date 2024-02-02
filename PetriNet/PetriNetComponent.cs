using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Places;
using TACPN.Transitions;

namespace TACPN;

public class PetriNetComponent
{
    public required ICollection<Transition> Transitions { get; init; }
    public required ICollection<CapacityPlace> CapacityPlaces { get; init; }
    public required ICollection<Places.Place> Places { get; init; }
    public required IEnumerable<string> InvolvedParts { get; set; } 

    public  HashSet<IPlace> AllPlaces()
    {
        var a = new HashSet<IPlace>(Places);
        a.UnionWith(CapacityPlaces);
        return a;
    }

    public required IEnumerable<ColourType> ColourTypes { get; init; }
    public required string Name { get; init; }
}

public class TimedArcColouredPetriNet
{
    public IEnumerable<Transition> Transitions { get; init; }
    public IEnumerable<CapacityPlace> CapacityPlaces { get; init; }
    public IEnumerable<Place> Places { get; init; }
    public IEnumerable<ColourVariable> Variables { get; init; }
    public required IEnumerable<ColourType> ColourTypes { get; init; }
    public required string Name { get; init; }

    public TimedArcColouredPetriNet(PetriNetComponent[] components)
    {
        this.Transitions = components.SelectMany(e => e.Transitions);
        this.CapacityPlaces = components.SelectMany(e => e.CapacityPlaces);
        this.Places = components.SelectMany(e => e.Places);
        this.CapacityPlaces = components.SelectMany(e => e.CapacityPlaces);
        ColourTypes = components.SelectMany(e => e.ColourTypes).ToHashSet();

        this.Variables = components.SelectMany(e => e.Transitions.SelectMany(transition =>
        {
            var ingoingVariables = transition.InGoing.Select(e => e.Expression.ColourValue)
                .OfType<IColourVariableExpression>()
                .Select(e => e.ColourVariable).ToHashSet();
            

            var outgoingVariables = transition.OutGoing.Select(e => e.Expression.ColourValue)
                .OfType<IColourVariableExpression>()
                .Select(e => e.ColourVariable).ToHashSet();

            return outgoingVariables.Union(ingoingVariables);
        }));

    }

}