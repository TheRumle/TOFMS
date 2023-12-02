using TACPN.Net.Transitions;

namespace TACPN.Net;

public record ColourType(string Name, IEnumerable<string> Colours);

public class PetriNetComponent
{
    public required ICollection<Transition> Transitions { get; init; }
    public required ICollection<Place> Places { get; init; }
    public required IEnumerable<string> Colors { get; init; }

    public required IEnumerable<ColourType> ColourTypes { get; init; }
}