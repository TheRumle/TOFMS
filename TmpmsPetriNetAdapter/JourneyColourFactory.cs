using TACPN.Colours.Type;
using Tmpms.Common;
using Tmpms.Common.Journey;

namespace TmpmsPetriNetAdapter;

public class JourneyColourFactory
{
    public static ColourType CreateJourneyColour(JourneyCollection journeyCollection)
    {
        IEnumerable<IEnumerable<Location>> journeys = journeyCollection.Select(e => e.Value);
        var longestJourney = journeys.MaxBy(e => e.Count());
        
        
        return new IntegerRangedColour(ColourType.JourneyColourName, longestJourney.Count());



    }
}