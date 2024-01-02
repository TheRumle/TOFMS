namespace Tofms.Common;

public class JourneyCollection : Dictionary<string, IEnumerable<KeyValuePair<int, Location>>>
{
    public JourneyCollection(IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> keyValuePairs):base(keyValuePairs)
    {
    }
    
    public static string ColourName = "Journey";
    

}