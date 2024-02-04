namespace Tmpms.Common;

public class IndexedJourneyCollection : Dictionary<string, IEnumerable<KeyValuePair<int, Location>>>
{
    public IndexedJourneyCollection(IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> keyValuePairs):base(keyValuePairs)
    {
    }
}
public static class JourneyExtensions
{
    
    public static IndexedJourneyCollection ToIndexedJourney(this IReadOnlyDictionary<string, IEnumerable<Location>> dict)
    {
        IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<int, Location>>>> j = dict.Select(e =>
        {
            var k = e.Key!;
            var values = e.Value;
            IEnumerable<KeyValuePair<int, Location>> newValues =
                values.Select((h, index) => KeyValuePair.Create(index, h));
            return KeyValuePair.Create(k, newValues);
        });

        return new IndexedJourneyCollection(j);
    }
}

public class JourneyCollection : Dictionary<string, IEnumerable<Location>>
{
    public JourneyCollection(IEnumerable<KeyValuePair<string, IEnumerable<Location>>> journey):base(journey)
    {
        
    }
    
    
    public static JourneyCollection ConstructJourneysFor((string partType, IEnumerable<Location> locations)[] journeys)
    {
        var journeyCollection = journeys
            .Select(e =>
            {
                return KeyValuePair.Create(e.partType, e.locations);
            })
            .ToDictionary();
        return new JourneyCollection(journeyCollection);
    }
    
    
}