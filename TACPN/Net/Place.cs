namespace TACPN.Net;

public class Place
{

    public TokenCollection Tokens { get; init; }

    public Place(string name, IEnumerable<KeyValuePair<string, int>> colorMaxAgeDict, string colourTypeName)
    {
        KeyValuePair<string, int>[] keyValuePairs = colorMaxAgeDict as KeyValuePair<string, int>[] ?? colorMaxAgeDict.ToArray();
        ColorInvariants = new Dictionary<string, int>(keyValuePairs);
        Name = name;
        Tokens = new TokenCollection()
        {
            Colours = keyValuePairs.Select(e => e.Key).ToList()
        };

        ColourType = new(colourTypeName, keyValuePairs.Select(e => e.Key));
    }

    public string Name { get; init; }
    public IDictionary<string, int> ColorInvariants { get; init; }

    public bool IsCapacityLocation { get; init; }

    public ColourType ColourType { get; } 

}