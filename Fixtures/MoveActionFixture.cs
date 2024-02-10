using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Places;
using TestDataGenerator;
using Tmpms.Common;
using Tmpms.Common.Journey;

namespace JsonFixtures;


public class MoveActionFixture
{
    public readonly static IEnumerable<string> Parts = ["P1", "P2", "P3"];
    public ColourType PartColourType = new("Parts", Parts);
    public Location ProcessingLocation { get; set; } = new("From", 3, new[]
    {
        new Invariant("P1", 0, 3),
        new Invariant("P2", 0, 5),
        new Invariant("P3", 3, 5)
    }, true);

    public Location BufferLocation { get; set; } = new("From", 3, Invariant.InfinityInvariantsFor(Parts), false);

    public static ColourVariable[] VariablesForParts(IDictionary<string, int> journeys)
    {
        return journeys.Select(e => CreateFromPartName(e.Key, e.Value))
            .ToArray();
    } 
    
    public static ColourVariable VariableForPart(string part, int maxValue)
    {
        return CreateFromPartName(part, maxValue);
    }


    public readonly LocationGenerator LocationGenerator;


    public MoveActionFixture()
    {
        LocationGenerator = new LocationGenerator(Parts);
    }

    public static ColourVariable CreateFromPartName(string partName, int numberOfPossibleValues)
    {
        return new ColourVariable(partName+"Var", new ColourType("Parts",
            Enumerable.Range(0,numberOfPossibleValues).Select(e=>e.ToString())));
    }

    public IEnumerable<Location> CreateLocations(int n, bool isProcessing = false)
    {
        var locations = this.LocationGenerator.GenerateLocations(n);
        return locations.Select(e =>
        {
            return e with { IsProcessing = isProcessing };
        });
    }
}