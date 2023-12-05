using TACPN.Net.Transitions;

namespace TACPN.Net;

public record ColourType(string Name, IEnumerable<string> Colours)
{
    public static readonly ColourType DefaultColorType = new ColourType("Dot", new[] { "dot" });
    public static readonly ColourType TokensColourType = new ColourType("Tokens",new []{"Tokens"});
}

public class PetriNetComponent
{
    public required ICollection<Transition> Transitions { get; init; }
    public required ICollection<CapacityPlace> CapacityPlaces { get; init; }
    public required ICollection<Place> Places { get; init; }

    public  HashSet<IPlace> AllPlaces()
    {
        var a = new HashSet<IPlace>(Places);
        a.UnionWith(CapacityPlaces);
        return a;
    }
    public required IEnumerable<string> Colors { get; init; }

    public required IEnumerable<ColourType> ColourTypes { get; init; }
    public required string Name { get; init; }
    public ColourType Journey { get; init; }
}