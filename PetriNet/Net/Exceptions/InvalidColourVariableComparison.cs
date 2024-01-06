using TACPN.Net.Colours.Values;

namespace TACPN.Net.Exceptions;

public class InvalidColourVariableComparison : ArgumentException
{

    public InvalidColourVariableComparison(ColourVariable variable, int value)
        : base($"{variable.Name} can never be {value}. The only possibilities are {string.Join(" ", variable.VariableValues.AsInts)}.")
    {
        
    }
}