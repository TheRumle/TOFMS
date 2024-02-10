using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TACPN.Places;
using TestDataGenerator;
using Tmpms.Common;

namespace JsonFixtures;


public class MoveActionFixture
{
    public readonly static IEnumerable<string> Parts = ["P1", "P2", "P3"];
    public readonly ColourType PartColourType = new("Parts", Parts);
    public readonly LocationGenerator LocationGenerator;
    public readonly Location BufferLocation = new("From", 3, Invariant.InfinityInvariantsFor(Parts), false);
    public readonly Location ProcessingLocation = new("From", 3, new[]
    {
        new Invariant("P1", 0, 3),
        new Invariant("P2", 0, 5),
        new Invariant("P3", 3, 5)
    }, true);
    
    public MoveActionFixture()
    {
        LocationGenerator = new LocationGenerator(Parts);
    }
    
    public static ColourVariable[] VariablesForParts(IDictionary<string, int> journeys)
    {
        return journeys.Select(e => CreateFromPartName(e.Key, e.Value))
            .ToArray();
    }

    public static ColourVariable VariableForPart(string part, int maxValue) => CreateFromPartName(part, maxValue);
    


    public static ColourVariable CreateFromPartName(string partName, int numberOfPossibleValues)
    {
        return new ColourVariable(partName+"Var", new ColourType("Parts",
            Enumerable.Range(0,numberOfPossibleValues).Select(e=>e.ToString())));
    }
}