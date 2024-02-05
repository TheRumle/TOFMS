namespace Tmpms.Common.Journey;

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

    public Dictionary<string, int> JourneyLengths()
    {
        return this.Select(e => KeyValuePair.Create(
            e.Key,
            e.Value.Count()
        )).ToDictionary();
    }
}