using TACPN.Net.Transitions;

namespace TACPN.Net;

public class PetriNetComponent
{
    public ICollection<Transition> Transitions { get; init; }
    public ICollection<Place> Places { get; init; }
    public IEnumerable<string> Colors { get; init; }

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