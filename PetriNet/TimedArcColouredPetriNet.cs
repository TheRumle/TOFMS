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
    public required IEnumerable<ColourVariable> Variables { get; init; }
    public required IEnumerable<ColourType> ColourTypes { get; init; }
    public required string Name { get; init; }

    public TimedArcColouredPetriNet(PetriNetComponent[] components)
    {
        Transitions = components.SelectMany(e => e.Transitions);
        CapacityPlaces = components.SelectMany(e => e.CapacityPlaces);
        Places = components.SelectMany(e => e.Places);
    }

}