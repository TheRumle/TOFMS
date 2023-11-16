namespace TACPN.Net;
public class Place
{
    public string Name { get; init; }
    public IDictionary<string, int> ColorInvariants { get; init; }

    public Place(string name, IEnumerable<KeyValuePair<string, int>> colorMaxAgeDict)
    {
        this.ColorInvariants = new Dictionary<string, int>(colorMaxAgeDict);
        Name = name;
    }
}