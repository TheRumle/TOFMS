using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Places;
using TACPN.Transitions;

namespace TACPN;

public class TimedArcColouredPetriNet
{
    public IEnumerable<Transition> Transitions { get; init; }
    public IEnumerable<CapacityPlace> CapacityPlaces { get; init; }
    public IEnumerable<Place> Places { get; init; }
    public IEnumerable<ColourVariable> Variables { get; init; }
    public IEnumerable<ColourType> ColourTypes { get; init; }
    public required string Name { get; init; }

    public TimedArcColouredPetriNet(PetriNetComponent[] components)
    {
        Transitions = components.SelectMany(e => e.Transitions);
        CapacityPlaces = components.SelectMany(e => e.CapacityPlaces);
        Places = components.SelectMany(e => e.Places);
        ColourTypes = components.SelectMany(e => e.ColourTypes).ToHashSet();

        Variables = components.SelectMany(e => e.Transitions.SelectMany(transition =>
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