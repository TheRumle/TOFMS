namespace Tmpms.Common.Journey;

public class IndexedJourneyCollection : Dictionary<string, IEnumerable<KeyValuePair<int, Location>>>
{
    public IndexedJourneyCollection(IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> keyValuePairs):base(keyValuePairs)
    {
    }
}