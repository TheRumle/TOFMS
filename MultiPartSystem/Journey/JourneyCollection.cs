namespace Tmpms.Journey;

public class JourneyCollection : Dictionary<string, IEnumerable<Location>>
{
    public IEnumerable<Location> Locations => Values.SelectMany(e => e);
    public IEnumerable<string> Parts => Keys;
    public JourneyCollection(IEnumerable<KeyValuePair<string, IEnumerable<Location>>> journey):base(journey)
    {
        
    }

    public JourneyCollection(): base(new List<KeyValuePair<string, IEnumerable<Location>>>())
    {
        
    }
    
    
    public static JourneyCollection ConstructJourneysFor((string partType, IEnumerable<Location> locations)[] journeys)
    {
        var journeyCollection = journeys
            .Select(e => (e.partType, e.locations))
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

    public IEnumerable<int> GetIndexOccurrencesFor(string part, Location location)
    {
        return this[part].Where(e => e == location).Select((loc, index) => index);
    }
}