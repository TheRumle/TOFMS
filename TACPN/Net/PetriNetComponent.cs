using TACPN.Net.Transitions;

namespace TACPN.Net;

public record ColourType(string Name, IEnumerable<string> Colours)
{
    public static readonly ColourType DefaultColorType = new ColourType("Dot", new[] { "dot" });
}

public class PetriNetComponent
{
    public required ICollection<Transition> Transitions { get; init; }
    public required ICollection<Place> Places { get; init; }
    public required IEnumerable<string> Colors { get; init; }

    public required IEnumerable<ColourType> ColourTypes { get; init; }
    public required string Name { get; init; }
}