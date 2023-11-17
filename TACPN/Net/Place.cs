using Common;

namespace TACPN.Net;

public class Place
{
    public static readonly string CapacityPlaceColor = "dot";

    public Place(string name, IEnumerable<KeyValuePair<string, int>> colorMaxAgeDict)
    {
        ColorInvariants = new Dictionary<string, int>(colorMaxAgeDict);
        Name = name;
    }

    public string Name { get; init; }
    public IDictionary<string, int> ColorInvariants { get; init; }

    public bool IsCapacityLocation { get; private set; }

    public static Place CapacityPlace(string name)
    {
        var kvp = KeyValuePair.Create(CapacityPlaceColor, (int)InfinityInteger.Positive);
        return new Place(name, new[] { kvp })
        {
            IsCapacityLocation = true
        };
    }
}