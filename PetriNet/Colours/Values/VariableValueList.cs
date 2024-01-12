using TACPN.Colours.Type;

namespace TACPN.Colours.Values;

public class VariableValueList
{
    public readonly IEnumerable<string> PossibleColourValues;
    public IEnumerable<int> AsInts => PossibleColourValues.Select((_, index) => index);
    
    public VariableValueList(ColourType colourType)
    {
        this.PossibleColourValues = colourType.ColourNames;
    }
}