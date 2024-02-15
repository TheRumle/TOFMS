namespace Tmpms.Journey;

public static class JourneyExtensions
{

    public static IndexedJourneyCollection ToIndexedJourney(
        this IReadOnlyDictionary<string, IEnumerable<Location>> dict)
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
    
    public static IndexedJourneyCollection ToIndexedJourney(
        this JourneyCollection dict)
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