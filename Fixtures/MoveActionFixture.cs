using System.Runtime.CompilerServices;
using TACPN.Colours.Type;
using TACPN.Colours.Values;
using TestDataGenerator;

namespace JsonFixtures;

public class MoveActionFixture
{
    public readonly static IEnumerable<string> Parts = ["P1", "P2", "P3"];
    public readonly ColourType PartColourType = new("Parts", Parts);

    
    public MoveActionFixture()
    {}
    
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
    
    public LocationGenerator CreateLocationGenerator(IEnumerable<string> partTypes)
    {
        return new LocationGenerator(partTypes);
    }
}