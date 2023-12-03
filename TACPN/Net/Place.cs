namespace TACPN.Net;

public class Place
{

    public TokenCollection Tokens { get; init; }

    public Place(string name, IEnumerable<KeyValuePair<string, int>> colorMaxAgeDict, ColourType colourType)
    {
        KeyValuePair<string, int>[] keyValuePairs = colorMaxAgeDict as KeyValuePair<string, int>[] ?? colorMaxAgeDict.ToArray();
        if (keyValuePairs.Length == 0) throw new ArgumentException($"Tried to construct place {name} with colour type {colourType}, but the given invariants where empty so the colour type could not be constructed!");
        
        ColorInvariants = new Dictionary<string, int>(keyValuePairs);
        Name = name;
        Tokens = new TokenCollection()
        {
            Colours = keyValuePairs.Select(e => e.Key).ToList()
        };

        ColourType = colourType;
    }

    public string Name { get; init; }
    public IDictionary<string, int> ColorInvariants { get; init; }

    public bool IsCapacityLocation { get; init; }

    public ColourType ColourType { get; } 

}