using TACPN.Net.Transitions;

namespace TACPN.Net;

public class PetriNetComponent
{
    public required ICollection<Transition> Transitions { get; init; }
    public required ICollection<Place> Places { get; init; }
    public required IEnumerable<string> Colors { get; init; }

    public static PetriNetComponent Empty()
    {
        return new PetriNetComponent
        {
            Colors = new List<string>(),
            Places = new List<Place>(),
            Transitions = new List<Transition>()
        };
    }
}