using TACPN.Net.Colours.Type;

namespace TACPN.Net.Colours.Values;

public class VariableValueList
{
    public readonly IEnumerable<string> PossibleColourValues;
    public IEnumerable<int> AsInts => PossibleColourValues.Select((_, index) => index);
    
    public VariableValueList(ColourType colourType)
    {
        this.PossibleColourValues = colourType.ColourNames;
    }
}